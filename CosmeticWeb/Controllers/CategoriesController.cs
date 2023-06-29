using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories!.ToListAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }
        #region Krijon kategorine me te dhenat nga forma

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("Id,Name,CreatedAt,ImageFile,ModifiedAt")] Category category)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(category.ImageFile!.FileName);
                string extension = Path.GetExtension(category.ImageFile.FileName);
                category.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/CategoryImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(fileSteam);
                }

                category.CreatedAt = DateTime.UtcNow;
                category.ModifiedAt = DateTime.UtcNow;
                category.Id = Guid.NewGuid();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #endregion

        #region Shfaq formen per te edituar nje kategori
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        #endregion

        #region Edito kategorine me te dhenat nga forma

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,CreatedAt,ImageFile,ModifiedAt")] Category category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Category? previousPath = await _context!.Categories!.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CategoryImages", previousPath!.Image!);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile!.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    category.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/CategoryImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(fileSteam);
                    }

                    category.ModifiedAt = DateTime.UtcNow;
                   
                    _context.Entry(previousPath).CurrentValues.SetValues(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        #endregion

        #region Shfaq formen per te fshire kategorine

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        #endregion

        #region Fshin kategorine kur useri e konfirmon 
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }

            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CategoryImages", category.Image!);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Shfaq produket ne momntin e klikimit te kategorise
        [Authorize]
        public async Task<IActionResult> Products(Guid id)
        {
            var products = await _context.Products!.Include(x => x.Category).Where(x => x.CategoryId == id).ToListAsync();
            var categoryName = await _context.Categories!.FirstOrDefaultAsync(x => x.Id == id);
            //ViewBag.CategoyName = categoryName!.Name;
            ViewData["CategoyName"] = categoryName.Name;
            return View(products);
        }
        #endregion

        #region Kthen true ose false nese kategoria me at id ekziston
        private bool CategoryExists(Guid id)
        {
            return _context.Categories!.Any(e => e.Id == id);
        }
        #endregion
    }
}