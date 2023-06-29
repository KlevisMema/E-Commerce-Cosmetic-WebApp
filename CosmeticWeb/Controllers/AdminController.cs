using CosmeticWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CosmeticWeb.Models;

namespace CosmeticWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _user;

        public AdminController
        (
            ApplicationDbContext context,
            UserManager<User> user
        )
        {
            _context = context;
            _user = user;
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Index()
        {
            //var users = await _context.Users.CountAsync();

            //var employees = await _user.GetUsersInRoleAsync("Employee");
            //var allEmployees = employees.Count();

            //var subsrciptions = await _context.Subscribtions!.CountAsync();

            //var income = await _context.OrderItems!.Select(x => x.Price * x.Quantity).SumAsync();

            //var getAllRecentUsers = await _user.GetUsersInRoleAsync("User");
            //var RecentUsers = getAllRecentUsers.Take(5);

            //var recentOrders = await _context.OrderItems!.Include(x => x.Order).Include(x => x.Product).ToListAsync();

            //ViewBag.Users = users;
            //ViewBag.Employees = allEmployees;
            //ViewBag.Subtitles = subsrciptions;
            //ViewBag.Income = income;
            //ViewBag.Recentusers = RecentUsers;
            //ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }
}