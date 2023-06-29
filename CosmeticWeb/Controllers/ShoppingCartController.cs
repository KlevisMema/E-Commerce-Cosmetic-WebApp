#region Usings
using CosmeticWeb.Data;
using CosmeticWeb.Helpers;
using CosmeticWeb.Models;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
#endregion

namespace CosmeticWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Injekto databazen ne kontroller

        private readonly ApplicationDbContext _context;

        public ShoppingCartController
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        #endregion

        #region Shfaq shopping cart me produkte 

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart") ?? new List<CartItemViewModel>();

            var cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            ViewData["grand_total"] = cartVM.GrandTotal;

            if (TempData["CategoryId"] is not null)
            {
                Guid? categoryid = (Guid)TempData["CategoryId"];

                if (categoryid.ToString() != "")
                {
                    var products = await _context.Products!.Include(x => x.Category).Where(x => x.CategoryId == categoryid).ToListAsync();

                    Random random = new();

                    ViewData["Suggestions"] = products.OrderBy(x => random.Next()).Take(4).ToList();
                }
            }
            else
            {
                Random random = new();

                var randomCategoryId = cart.Select(x => x.ProductCategoryId).OrderBy(x => random.Next()).FirstOrDefault();

                var products = await _context.Products!.Include(x => x.Category)
                                                       .Where(x => x.CategoryId == randomCategoryId)
                                                       .ToListAsync();

                ViewBag.SuggestionsV2 = products.OrderBy(x => random.Next()).Take(4);
            }

            

            return View(cartVM);
        }

        #endregion

        #region Shton nje produkt ne shopping cart 

        [Authorize]
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

            TempData["CategoryId"] = product!.CategoryId;

            return RedirectToAction("Index");
        }

        #endregion

        #region Heq 1 nga sasia e nje produkti nga shopping carta 

        [Authorize]
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

        #endregion

        #region Heq te gjithe produktin nga shopping cart pavarsisht sasise

        [Authorize]
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

        #endregion

        #region I heq te gjithe produktet nga shopping carta   

        [Authorize]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("ShoppingCart");

            return RedirectToAction("Index");
        }

        #endregion

        #region Shfaq formen e check outit 

        [HttpGet]
        [Authorize]
        public IActionResult ProccedToCheckout()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("ShoppingCart");

            ViewData["grand_total"] = cart!.Sum(x => x.Price * x.Quantity);

            return View();

        }

        #endregion

        #region Ploteson tabelen order te dhenat nga forma e check out, dhe order items me produktet e bera order nga useri 

        [HttpPost]
        [Authorize]
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

        #endregion

        #region Shfaq pamjen e thank you pasi useri ka bere check out 

        [Authorize]
        public IActionResult ThankYou()
        {
            return View();
        }

        #endregion

    }
}