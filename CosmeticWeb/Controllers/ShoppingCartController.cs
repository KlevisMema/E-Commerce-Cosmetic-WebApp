using CosmeticWeb.Data;
using CosmeticWeb.Helpers;
using CosmeticWeb.Models;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            //var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            //var cartVM = new CartViewModel
            //{
            //    CartItems = cart,
            //    GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            //};

            //ViewData["grand_total"] = cartVM.GrandTotal;

            return View(/*cartVM*/);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Add(int id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var cartItem = cart.Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            if (cartItem == null)
                cart.Add(new CartItemViewModel(product, quantity));
            else
            {
                if (quantity == 0)
                    cartItem.Quantity += 1;
                else
                    cartItem.Quantity += quantity;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Decrease(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            var cartItem = cart.Where(x => x.ProductId.Equals(id)).FirstOrDefault();

            if (cartItem.Quantity > 1)
                --cartItem.Quantity;
            else
                cart.RemoveAll(x => x.ProductId.Equals(id));

            if (cart.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            cart.RemoveAll(x => x.ProductId.Equals(id));

            if (cart.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ProccedToCheckout()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            ViewData["grand_total"] = cart.Sum(x => x.Price * x.Quantity);

            return View("CheckOutCompleted");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ProccedToCheckout(IFormCollection frm_coll)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            var order = new Order()
            {
                CustomerName = frm_coll["Name"],
                CustomerPhone = frm_coll["Phone"],
                CustomerEmail = frm_coll["Email"],
                CustomerAddress = frm_coll["Adress"]
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var stCart in cart)
            {
                OrderItem orderDetail = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = stCart.ProductId,
                    Quantity = stCart.Quantity,
                    Price = stCart.Price
                };
                _context.OrderItems.Add(orderDetail);
                _context.SaveChanges();
            }
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("ThankYou");
        }

        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}