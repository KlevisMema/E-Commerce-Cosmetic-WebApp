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

        public async Task<IActionResult> Details(Guid? id)
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
                return RedirectToAction(nameof(Index));
            }
            return View(subscribe);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Subscribtions == null)
            {
                return NotFound();
            }

            var subscribe = await _context.Subscribtions.FindAsync(id);
            if (subscribe == null)
            {
                return NotFound();
            }
            return View(subscribe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Email,SubscribedAt")] Subscribe subscribe)
        {
            if (id != subscribe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subscribe.SubscribedAt = DateTime.UtcNow;
                    _context.Update(subscribe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscribeExists(subscribe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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