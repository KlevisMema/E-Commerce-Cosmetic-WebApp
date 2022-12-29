using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticWeb.Controllers
{
    public class SubscribtionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscribtionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Subscribtions.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,SubscribedAt")] Subscribe subscribe)
        {
            if (ModelState.IsValid)
            {
                subscribe.Id = Guid.NewGuid();
                subscribe.SubscribedAt = DateTime.UtcNow;
                _context.Add(subscribe);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(subscribe);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Subscribtions == null)
            {
                return NotFound();
            }

            var subscribe = await _context.Subscribtions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscribe == null)
            {
                return NotFound();
            }

            return View(subscribe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Subscribtions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subscribtions'  is null.");
            }
            var subscribe = await _context.Subscribtions.FindAsync(id);
            if (subscribe != null)
            {
                _context.Subscribtions.Remove(subscribe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscribeExists(Guid id)
        {
          return _context.Subscribtions.Any(e => e.Id == id);
        }
    }
}