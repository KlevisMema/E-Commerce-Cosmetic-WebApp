using CosmeticWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CosmeticWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.CountAsync();

            var employees = await _user.GetUsersInRoleAsync("Employee");
            var allEmployees = employees.Count();

            var subsrciptions = await _context.Subscribtions.CountAsync();

            var income = await _context.OrderItems.Select(x => x.Price).SumAsync();

            var recentusers = await _context.Users.ToListAsync();

            var recentOrders = await _context.OrderItems.Include(x => x.Order).Include(x => x.Product).ToListAsync();

            ViewBag.Users = users;
            ViewBag.Employees = allEmployees;
            ViewBag.Subtitles = subsrciptions;
            ViewBag.Income = income;
            ViewBag.Recentusers = recentusers;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}