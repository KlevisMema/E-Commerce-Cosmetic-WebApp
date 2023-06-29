using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class TestimonialsController : Controller
    {
        #region Injekto databazen dhe IWebHostEnvironment per imazhet ne kontroller

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public TestimonialsController
        (
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment
        )
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        #endregion

        #region shfaq listen e testimonials
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Testimonials!.ToListAsync());
        }
        #endregion

        #region shfaq formen per te krijuar nje testimonial
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region testimonail eshte krijuar dhe ruhet ne databaze 
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Rating,Description,ImageFile,CreatedAt")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(testimonial.ImageFile!.FileName);
                string extension = Path.GetExtension(testimonial.ImageFile.FileName);
                testimonial.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/TestimonialsImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await testimonial.ImageFile.CopyToAsync(fileSteam);
                }

                testimonial.Id = Guid.NewGuid();
                testimonial.CreatedAt = DateTime.Now;
                _context.Add(testimonial);
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
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }
        #endregion

        #region konfirmohet fshirja
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Testimonials'  is null.");
            }

            var testimonial = await _context.Testimonials.FindAsync(id);

            if (testimonial != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\TestimonialsImages", testimonial.Image!);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}