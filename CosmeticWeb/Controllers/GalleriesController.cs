using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticWeb.Controllers
{
    public class GalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public GalleriesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Galleries.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageFile,DateCreated")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(gallery.ImageFile.FileName);
                string extension = Path.GetExtension(gallery.ImageFile.FileName);
                gallery.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/GallerieImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                    await gallery.ImageFile.CopyToAsync(fileSteam);

                gallery.Id = Guid.NewGuid();
                gallery.DateCreated = DateTime.UtcNow;
                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Galleries == null)
                return NotFound();

            var gallery = await _context.Galleries.FindAsync(id);

            if (gallery == null)
                return NotFound();
            return View(gallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ImageFile,DateCreated")] Gallery gallery)
        {
            if (id != gallery.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.Galleries.FirstOrDefaultAsync(x => x.Id.Equals(id));


                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\GallerieImages", previousPath.Image);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(gallery.ImageFile.FileName);
                    string extension = Path.GetExtension(gallery.ImageFile.FileName);
                    gallery.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/GallerieImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        await gallery.ImageFile.CopyToAsync(fileSteam);
                    }

                    gallery.DateCreated = DateTime.UtcNow;
                    _context.Entry(previousPath).CurrentValues.SetValues(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gallery);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Galleries == null)
                return NotFound();

            var gallery = await _context.Galleries.FirstOrDefaultAsync(m => m.Id == id);

            if (gallery == null)
                return NotFound();

            return View(gallery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Galleries == null)
                return Problem("Entity set 'ApplicationDbContext.Galleries'  is null.");

            var gallery = await _context.Galleries.FindAsync(id);

            if (gallery != null)
            {

                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\GallerieImages", gallery.Image);

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                _context.Galleries.Remove(gallery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(Guid id)
        {
            return _context.Galleries.Any(e => e.Id == id);
        }
    }
}