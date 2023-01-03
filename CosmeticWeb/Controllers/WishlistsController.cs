using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CosmeticWeb.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Wishlists.Where(x => x.UserId.Equals(Guid.Parse(user))).ToListAsync());
        }

        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Create(string productId)
        {
            Wishlist wishlist = new();
            if (ModelState.IsValid)
            {
               var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var product = _context.Products.FirstOrDefault(x => x.Id.Equals(Guid.Parse(productId)));

                wishlist.Id = Guid.NewGuid();
                wishlist.ProductId = product.Id;
                wishlist.Image = product.Image;
                wishlist.Name = product.Name;
                wishlist.Price = product.Price;
                wishlist.PreviousPrice = product.PreviousPrice;
                wishlist.UserId = Guid.Parse(user);

                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishlist);
        }

        private bool WishlistExists(Guid id)
        {
            return _context.Wishlists.Any(e => e.Id == id);
        }
    }
}