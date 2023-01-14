using CosmeticWeb.Data;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticWeb.Controllers
{
    public class UsersController : Controller
    {
        #region Injekto databazen dhe UserManager<IdentityUser> per veprime me indentity

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController
        (
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager
        )
        {
            _db = db;
            _userManager = userManager;
        }

        #endregion

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");

            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");

            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(UserViewModel identityUser)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Email = identityUser.Email;
                user.UserName = identityUser.Email;

                var result = await _userManager.CreateAsync(user, identityUser.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction("Index");

                }
            }
            return View(identityUser);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            _db.Users.Remove(user!);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

    }
}