#region
using CosmeticWeb.Data;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CosmeticWeb.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
#endregion

namespace CosmeticWeb.Controllers
{
    public class UsersController : Controller
    {
        #region Injekto databazen dhe UserManager<User> per veprime me indentity

        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController
        (
            ApplicationDbContext db,
            UserManager<User> userManager,
            IWebHostEnvironment hostEnvironment,
            RoleManager<IdentityRole> roleManager
        )
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _HostEnvironment = hostEnvironment;
        }

        #endregion

        #region Menaxhimi i userave : Metodat

        #region Marr te gjithe userat ne rolin Employee dhe i shfaqim tek viewja Index 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");

            List<UsersViewModel> usersViewModel = new List<UsersViewModel>();

            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);

                usersViewModel.Add(new UsersViewModel { Role = roles.FirstOrDefault()!, Id = item.Id, UserName = item.UserName, Email = item.Email, EmailConfirmed = item.EmailConfirmed });
            }

            return View(usersViewModel);
        }
        #endregion

        #region Marr te gjithe userat ne rolin User  dhe i shfaqim tek viewja Users 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _db.Users.ToListAsync();

            List<UsersViewModel> usersViewModel = new List<UsersViewModel>();

            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);

                if (roles.FirstOrDefault() != "Employee" && roles.FirstOrDefault() != "Admin")
                {
                    usersViewModel.Add(new UsersViewModel { Role = roles.FirstOrDefault()!, Id = item.Id, UserName = item.UserName, Email = item.Email, EmailConfirmed = item.EmailConfirmed });
                }
            }

            return View(usersViewModel);
        }
        #endregion

        #region shfaq formen per te krijuar nje user te ri
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            return View();
        }
        #endregion

        #region krijon userin e ri me te dhenat e marra nga forma
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
        #endregion

        #region fshin userin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            _db.Users.Remove(user!);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region Ngarkon foton per userin
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null)
                return BadRequest("Image file is required");

            var loggedUser = await _userManager.FindByNameAsync(User.Identity!.Name);

            if (loggedUser != null)
            {
                if (loggedUser.Image != null)
                {
                    string? imagePath = Path.Combine(_HostEnvironment.WebRootPath + "\\UsersProfileImages", loggedUser.Image);

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                string wwwRootPath = _HostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                loggedUser.Image = fileName += DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/UsersProfileImages", fileName);

                using (var fileSteam = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(fileSteam);

                await _userManager.UpdateAsync(loggedUser);

                return Ok();
            }

            return BadRequest("User doesn't exists");
        }
        #endregion

        #region Marr userin dhe shfaq viewn GetEditUser per te edituar userin.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEditUser(string id, string page)
        {
            var user = await _userManager.FindByIdAsync(id);

            var roles = await _userManager.GetRolesAsync(user);

            UsersViewModel usersViewModel = new()
            {
                Id = id,
                Role = roles.FirstOrDefault()!,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Page = page
            };

            ViewData["Roles"] = new SelectList(_db.Roles.ToList(), "Id", "Name");

            return View(usersViewModel);
        }
        #endregion

        #region Edito te dhenat e userit dhe kthejemi tek view e Users.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostEditUser(UsersViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id);

            if (user == null)
                return NotFound();

            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == userViewModel.NewRole);

            if (role == null)
                return NotFound();

            try
            {
                var result = await _userManager.RemoveFromRoleAsync(user, userViewModel.Role);
            }
            catch (Exception ex)
            {
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            user.Email = userViewModel.Email;
            user.UserName = userViewModel.UserName;
            user.EmailConfirmed = userViewModel.EmailConfirmed;

            await _userManager.UpdateAsync(user);

            if (userViewModel.Role == "User")
                return RedirectToAction("Index");
            else
                return RedirectToAction("Users");

        }
        #endregion

        #endregion

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        #region Menaxhimi i  roleve : Metodat  

        #region Marrim te gjithe rolet dhe i shfaqim ne view GetRoles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            return View(await _db.Roles.ToListAsync());
        }
        #endregion

        #region Shfaq viewn CreateRole me formen per te krijuar nje rol
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        #endregion

        #region Krijon rolin dhe kthehemi tek viewja GetRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            IdentityRole newRole = new()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
            };

            await _roleManager.CreateAsync(newRole);

            return RedirectToAction("GetRoles");
        }
        #endregion

        #region Marrim rolin dhe shfaqim viewn EditRole per te edituar rolin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            if (role == null)
                return RedirectToAction("GetRoles");

            return View(role);
        }
        #endregion

        #region Edit rolin dhe kthehemi tek viewja GetRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostEdit(string roleName, string Id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            role!.Name = roleName;

            await _roleManager.UpdateAsync(role);

            return RedirectToAction("GetRoles");
        }
        #endregion

        #region Marrim rolin dhe shfaqim viewn GetDelete
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDelete(string Id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            if (role == null)
                return RedirectToAction("GetRoles");

            return View(role);
        }
        #endregion

        #region Fshin rolin edhe kthehemi tek viewja GetRoles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostDelete(string Id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            if (role == null)
                return RedirectToAction("GetRoles");

            await _roleManager.DeleteAsync(role);

            return RedirectToAction("GetRoles");
        }
        #endregion

        #endregion

    }
}