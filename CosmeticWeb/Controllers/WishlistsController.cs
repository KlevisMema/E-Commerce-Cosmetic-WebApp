using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
              return View(await _context.Wishlists.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                wishlist.Id = Guid.NewGuid();
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