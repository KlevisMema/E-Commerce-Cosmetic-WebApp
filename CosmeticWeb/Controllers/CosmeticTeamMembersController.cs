﻿using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class CosmeticTeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public CosmeticTeamMembersController
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
            return View(await _context.CosmeticTeamMembers!.ToListAsync());
        }

        #region Shfaq formen per te krijuar nje member te ri
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Krijon member te ri me te dhenat nga forma
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("Id,Name,Role,Linkedin,Facebook,Instagram,Twitter,ImageFile,DateCreated")] CosmeticTeamMember cosmeticTeamMember)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(cosmeticTeamMember.ImageFile!.FileName);
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
        #endregion

        #region shfaq formen per te edituar nje team member
        [Authorize(Roles = "Admin,Employee")]
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
        #endregion

        #region Editon nje member
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Role,Linkedin,Facebook,Instagram,Twitter,ImageFile,DateCreated")] CosmeticTeamMember cosmeticTeamMember)
        {
            if (id != cosmeticTeamMember.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var previousPath = await _context.CosmeticTeamMembers!.FirstOrDefaultAsync(x => x.Id.Equals(id));

                    var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CosmeticTeamMemberImages", previousPath!.Image!);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);

                    string wwwRootPath = _HostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(cosmeticTeamMember.ImageFile!.FileName);
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
        #endregion

        #region Shfaq formen per te fshire nje member
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CosmeticTeamMembers == null)
                return NotFound();

            var cosmeticTeamMember = await _context.CosmeticTeamMembers.FirstOrDefaultAsync(m => m.Id == id);
            if (cosmeticTeamMember == null)
                return NotFound();

            return View(cosmeticTeamMember);
        }
        #endregion

        #region Fshin nje member
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.CosmeticTeamMembers == null)
                return Problem("Entity set 'ApplicationDbContext.CosmeticTeamMembers'  is null.");

            var cosmeticTeamMember = await _context.CosmeticTeamMembers.FindAsync(id);

            if (cosmeticTeamMember != null)
            {
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\CosmeticTeamMemberImages", cosmeticTeamMember.Image!);

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                _context.CosmeticTeamMembers.Remove(cosmeticTeamMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Kthen true ose false nqs memberi me at id egziston
        private bool CosmeticTeamMemberExists(Guid id)
        {
            return _context.CosmeticTeamMembers!.Any(e => e.Id == id);
        }
        #endregion
    }
}