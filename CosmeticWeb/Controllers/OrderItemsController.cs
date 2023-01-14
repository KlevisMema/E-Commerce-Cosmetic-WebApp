using CosmeticWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderItems!.ToListAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.OrderItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderItems'  is null.");
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems!.Any(e => e.Id.Equals(id));
        }
    }
}