#region
using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization; 
#endregion

namespace CosmeticWeb.Controllers
{
    public class ProductsController : Controller
    {
        #region Injekto databazen dhe IWebHostEnvironment per foton ne konstruktor

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public ProductsController
        (
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment
        )
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        #endregion

        #region Merr te gjithe produkete nga databaza

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products!.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        #endregion

        #region Shfaq formen per te krijuar nje produkt te ri 

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        #endregion

        #region Krijo produktin me te dhenat nga forma 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create
        (
            [Bind("Id,Name,Price,PreviousPrice,Description,Rating,CreatedAt,ModifiedAt,ImageFile,CategoryId")] Product product
        )
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile!.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);
                product.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/CreatedProductsImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                    await product.ImageFile.CopyToAsync(fileSteam);

                product.Id = Guid.NewGuid();
                product.CreatedAt = DateTime.UtcNow;
                product.ModifiedAt = DateTime.UtcNow;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        #endregion

        #region Shfaq formen per te edituar nje produkt

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        #endregion

        #region Edito produktin me te dhenat nga forma

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit
        (
            Guid id,
            [Bind("Id,Name,Price,PreviousPrice,Description,Rating,CreatedAt,ModifiedAt,ImageFile,CategoryId")] Product product
        )
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.Products!.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CreatedProductsImages", previousPath!.Image!);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile!.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/CreatedProductsImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                        await product.ImageFile.CopyToAsync(fileSteam);

                    product.ModifiedAt = DateTime.Now;
                    _context.Entry(previousPath).CurrentValues.SetValues(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        #endregion

        #region Shfaq formen per te fshire nje produkt

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        #endregion

        #region Fshi produkin kur user e konfirmon

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Products == null)
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");

            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CreatedProductsImages", product.Image!);

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Kthen true ose false nqs produkti me at id egziston

        private bool ProductExists(Guid id)
        {
            return _context.Products!.Any(e => e.Id == id);
        }

        #endregion

        public async Task<IActionResult> AllProducts(string? filterValue = null)
        {
            if (!String.IsNullOrEmpty(filterValue))
            {
                if (filterValue == "Price")
                {
                    var productsOrderedByPrice = await _context.Products!.OrderByDescending(e => e.Price).ToListAsync();

                    return View(productsOrderedByPrice);
                }
                if (filterValue == "Rating")
                {
                    var productsOrderedByRating = await _context.Products!.OrderByDescending(e => e.Rating).ToListAsync();

                    return View(productsOrderedByRating);
                }
            }
            else
            {
                var allProducts = await _context.Products!.ToListAsync();
                return View(allProducts);
            }
            return View();
        }

        public async Task<IActionResult> Search([FromForm] IFormCollection frm_coll)
        {
            var products = new List<Product>();

            if (!String.IsNullOrEmpty(frm_coll["productName"]))
            {
                products = await _context.Products!.Where(x => x.Name!.Contains(frm_coll["productName"])).ToListAsync();
            }

            ViewBag.ProductNameSearched = frm_coll["productName"];

            return View(products);
        }

        public async Task<IActionResult> Filter(IFormCollection frm_coll)
        {
            var products = new List<Product>();

            if (!String.IsNullOrEmpty(frm_coll["selectedValue"]))
            {
                string selectedValue = frm_coll["selectedValue"].ToString();

                if (selectedValue == "Price")
                {
                    products = await _context.Products!.OrderBy(x => x.Price).ToListAsync();
                }
                if (selectedValue == "Rating")
                {
                    products = await _context.Products!.OrderBy(x => x.Rating).ToListAsync();
                }
            }

            return View("AllProducts", products);
        }
    }
}