using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticWeb.Controllers
{
    public class CosmeticTeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public CosmeticTeamMembersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CosmeticTeamMembers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Role,Linkedin,Facebook,Instagram,Twitter,ImageFile,DateCreated")] CosmeticTeamMember cosmeticTeamMember)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(cosmeticTeamMember.ImageFile.FileName);
                string extension = Path.GetExtension(cosmeticTeamMember.ImageFile.FileName);
                cosmeticTeamMember.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/CosmeticTeamMemberImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                    await cosmeticTeamMember.ImageFile.CopyToAsync(fileSteam);

                cosmeticTeamMember.Id = Guid.NewGuid();
                cosmeticTeamMember.DateCreated = DateTime.UtcNow;
                _context.Add(cosmeticTeamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cosmeticTeamMember);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CosmeticTeamMembers == null)
            {
                return NotFound();
            }

            var cosmeticTeamMember = await _context.CosmeticTeamMembers.FindAsync(id);
            if (cosmeticTeamMember == null)
            {
                return NotFound();
            }
            return View(cosmeticTeamMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Role,Linkedin,Facebook,Instagram,Twitter,ImageFile,DateCreated")] CosmeticTeamMember cosmeticTeamMember)
        {
            if (id != cosmeticTeamMember.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.CosmeticTeamMembers.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CosmeticTeamMemberImages", previousPath.Image);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(cosmeticTeamMember.ImageFile.FileName);
                    string extension = Path.GetExtension(cosmeticTeamMember.ImageFile.FileName);
                    cosmeticTeamMember.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/CosmeticTeamMemberImages", fileName);

                    using (var fileSteam = new FileStream(path, FileMode.Create))
                        await cosmeticTeamMember.ImageFile.CopyToAsync(fileSteam);

                    cosmeticTeamMember.DateCreated = DateTime.UtcNow;

                    _context.Entry(previousPath).CurrentValues.SetValues(cosmeticTeamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CosmeticTeamMemberExists(cosmeticTeamMember.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cosmeticTeamMember);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CosmeticTeamMembers == null)
                return NotFound();

            var cosmeticTeamMember = await _context.CosmeticTeamMembers.FirstOrDefaultAsync(m => m.Id == id);
            if (cosmeticTeamMember == null)
                return NotFound();

            return View(cosmeticTeamMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CosmeticTeamMembers == null)
                return Problem("Entity set 'ApplicationDbContext.CosmeticTeamMembers'  is null.");

            var cosmeticTeamMember = await _context.CosmeticTeamMembers.FindAsync(id);

            if (cosmeticTeamMember != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CosmeticTeamMemberImages", cosmeticTeamMember.Image);

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                _context.CosmeticTeamMembers.Remove(cosmeticTeamMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CosmeticTeamMemberExists(Guid id)
        {
            return _context.CosmeticTeamMembers.Any(e => e.Id == id);
        }
    }
}