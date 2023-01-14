using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class WishlistsController : Controller
    {
        #region Injekto databazen ne kontroller 

        private readonly ApplicationDbContext _context;

        public WishlistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Shfaq pamjen e wishlistes me produktet nese ka 

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Wishlists!.Where(x => x.UserId.Equals(Guid.Parse(user))).ToListAsync());
        }

        #endregion

        #region Shton nje produkt ne wishliste 

        [Authorize]
        public async Task<IActionResult> Create(string productId)
        {
            Wishlist wishlist = new();
            if (ModelState.IsValid)
            {
                var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var existingProduct = _context.Wishlists!.Where(x => x.UserId.Equals(Guid.Parse(user))).Any(x => x.ProductId.Equals(Guid.Parse(productId)));

                if (!existingProduct)
                {
                    var product = _context.Products!.FirstOrDefault(x => x.Id.Equals(Guid.Parse(productId)));

                    wishlist.Id = Guid.NewGuid();
                    wishlist.ProductId = product!.Id;
                    wishlist.Image = product.Image;
                    wishlist.Name = product.Name;
                    wishlist.Price = product.Price;
                    wishlist.PreviousPrice = product.PreviousPrice;
                    wishlist.UserId = Guid.Parse(user);

                    _context.Add(wishlist);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Heq nje produkt nga wishlista

        [Authorize]
        public async Task<IActionResult> Remove(Guid id)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = await _context.Wishlists!.Where(x => x.UserId.Equals(Guid.Parse(user))).ToListAsync();

            var existing = wishlist.FirstOrDefault(x => x.ProductId == id);

            if (wishlist == null)
                return RedirectToAction(nameof(Index));

            _context.Wishlists!.Remove(existing!);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}