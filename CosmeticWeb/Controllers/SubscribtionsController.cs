using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class SubscribtionsController : Controller
    {
        #region Injekto databazen ne kontroller 

        private readonly ApplicationDbContext _context;

        public SubscribtionsController
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        #endregion

        #region kthen pamjen Index
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subscribtions!.ToListAsync());
        }
        #endregion

        #region krijon subscribe te ri dhe e ruan ne databaze
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection frm_coll)
        {
            if (ModelState.IsValid)
            {
                var subscribe = new Subscribe
                {
                    SubscribedAt = DateTime.Now,
                    Email = frm_coll["Email"],
                    Id = Guid.NewGuid()
                };

                _context.Add(subscribe);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region shfaq view e fshirjes "Are you sure you want to delete"
        [Authorize(Roles = "Admin,Employee")]
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
        #endregion

        #region konfirmohet fshirja
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Employee")]
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
        #endregion

    }
}