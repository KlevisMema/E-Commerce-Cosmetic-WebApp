using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class ContactUsController : Controller
    {
        #region Injekto databazen ne kontroller 
        private readonly ApplicationDbContext _context;

        public ContactUsController
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
            return View(await _context.ContactUs!.ToListAsync());
        }
        #endregion

        #region forma e contact us
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region krijon contact us 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([Bind("Id,Name,Subject,DateCreated,Email,Message")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                contactUs.DateCreated = DateTime.UtcNow;
                contactUs.Id = Guid.NewGuid();
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region shfaq view e fshirjes "Are you sure you want to delete"
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ContactUs == null)
            {
                return NotFound();
            }

            var contactUs = await _context.ContactUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            return View(contactUs);
        }
        #endregion

        #region konfirmohet fshirja
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ContactUs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContactUs'  is null.");
            }
            var contactUs = await _context.ContactUs.FindAsync(id);
            if (contactUs != null)
            {
                _context.ContactUs.Remove(contactUs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}