using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class HomeSlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public HomeSlidersController
        (
            ApplicationDbContext context, 
            IWebHostEnvironment hostEnvironment
        )
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.homeSliders!.ToListAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("Id,Name,Comment,ReadMore,ImageFile")] HomeSlider homeSlider)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(homeSlider.ImageFile!.FileName);
                string extension = Path.GetExtension(homeSlider.ImageFile.FileName);
                homeSlider.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/HomeSliderImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await homeSlider.ImageFile.CopyToAsync(fileSteam);
                }

                homeSlider.Id = Guid.NewGuid();
                _context.Add(homeSlider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeSlider);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.homeSliders == null)
            {
                return NotFound();
            }

            var homeSlider = await _context.homeSliders.FindAsync(id);
            if (homeSlider == null)
            {
                return NotFound();
            }
            return View(homeSlider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Comment,ReadMore,ImageFile")] HomeSlider homeSlider)
        {
            if (id != homeSlider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.homeSliders!.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\HomeSliderImages", previousPath!.Image!);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(homeSlider.ImageFile!.FileName);
                    string extension = Path.GetExtension(homeSlider.ImageFile.FileName);
                    homeSlider.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/HomeSliderImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        await homeSlider.ImageFile.CopyToAsync(fileSteam);
                    }

                    _context.Entry(previousPath).CurrentValues.SetValues(homeSlider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeSliderExists(homeSlider.Id))
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
            return View(homeSlider);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.homeSliders == null)
            {
                return NotFound();
            }

            var homeSlider = await _context.homeSliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeSlider == null)
            {
                return NotFound();
            }

            return View(homeSlider);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.homeSliders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.homeSliders'  is null.");
            }

            var homeSlider = await _context.homeSliders.FindAsync(id);

            if (homeSlider != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\HomeSliderImages", homeSlider.Image!);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.homeSliders.Remove(homeSlider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeSliderExists(Guid id)
        {
          return _context.homeSliders!.Any(e => e.Id == id);
        }
    }
}