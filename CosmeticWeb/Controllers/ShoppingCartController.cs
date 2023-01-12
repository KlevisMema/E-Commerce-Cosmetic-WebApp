using CosmeticWeb.Data;
using CosmeticWeb.Helpers;
using CosmeticWeb.Models;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart") ?? new List<CartItemViewModel>();

            var cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            ViewData["grand_total"] = cartVM.GrandTotal;

            return View(cartVM);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Add(Guid id, int quantity)
        {
            var product = await _context.Products!.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart") ?? new List<CartItemViewModel>();

            var cartItem = cart.Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            if (cartItem == null)
                cart.Add(new CartItemViewModel(product!, quantity));
            else
            {
                if (quantity == 0)
                    cartItem.Quantity += 1;
                else
                    cartItem.Quantity += quantity;
            }

            HttpContext.Session.SetJson("ShoppingCart", cart);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Decrease(Guid id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart");

            var cartItem = cart!.Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            if (cartItem!.Quantity > 1)
                --cartItem.Quantity;
            else
                cart!.RemoveAll(x => x.ProductId.Equals(id));

            if (cart!.Count == 0)
                HttpContext.Session.Remove("ShoppingCart");
            else
                HttpContext.Session.SetJson("ShoppingCart", cart!);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Remove(Guid id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart");

            cart!.RemoveAll(x => x.ProductId.Equals(id));

            if (cart.Count == 0)
                HttpContext.Session.Remove("ShoppingCart");
            else
                HttpContext.Session.SetJson("ShoppingCart", cart);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("ShoppingCart");

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ProccedToCheckout()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart");

            ViewData["grand_total"] = cart!.Sum(x => x.Price * x.Quantity);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ProccedToCheckout(ProceedCheckOutViewModel checkOut)
        {
            if (ModelState.IsValid)
            {
                var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart");

                var order = new Order()
                {
                    CustomerName = checkOut.Name,
                    CustomerPhone = checkOut.PhoneNumber,
                    CustomerEmail = checkOut.Email,
                    CustomerAddress = checkOut.Address,
                    CreatedDate = DateTime.Now
                };

                _context.Orders!.Add(order);
                _context.SaveChanges();

                foreach (var stCart in cart!)
                {
                    OrderItem orderDetail = new OrderItem()
                    {
                        OrderId = order.Id,
                        ProductId = stCart.ProductId,
                        Quantity = stCart.Quantity,
                        Price = stCart.Price
                    };

                    _context.OrderItems!.Add(orderDetail);
                    _context.SaveChanges();
                }

                HttpContext.Session.Remove("ShoppingCart");

                return RedirectToAction("ThankYou");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}