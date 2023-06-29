using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public BlogsController
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
            return View(await _context.Blogs!.ToListAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile,DateCreated,DateModified")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile!.FileName);
                string extension = Path.GetExtension(blog.ImageFile.FileName);
                blog.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/BlogsImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await blog.ImageFile.CopyToAsync(fileSteam);
                }

                blog.Id = Guid.NewGuid();
                blog.DateCreated = DateTime.Now;
                blog.DateModified = DateTime.Now;
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,ImageFile,DateCreated,DateModified")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.Blogs!.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\BlogsImages", previousPath!.Image!);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(blog.ImageFile!.FileName);
                    string extension = Path.GetExtension(blog.ImageFile.FileName);
                    blog.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/BlogsImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        await blog.ImageFile.CopyToAsync(fileSteam);
                    }


                    blog.DateModified = DateTime.UtcNow;
                    _context.Entry(previousPath).CurrentValues.SetValues(blog);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            return View(blog);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\BlogsImages", blog.Image!);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _context.Blogs.Remove(blog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(Guid id)
        {
            return _context.Blogs!.Any(e => e.Id == id);
        }
    }
}