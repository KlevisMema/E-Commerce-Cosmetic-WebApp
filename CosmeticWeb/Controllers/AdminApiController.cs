#region
using CosmeticWeb.Data;
using CosmeticWeb.Models;
using CosmeticWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
#endregion

namespace CosmeticWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        #region
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public AdminApiController
        (
            ApplicationDbContext db,
            UserManager<User> userManager
        )
        {
            _db = db;
            _userManager = userManager;
        }
        #endregion

        #region
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var numberOfUsers = await _db.Users.CountAsync();

            return Ok(numberOfUsers);
        }

        [HttpGet("get-employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var Users = await _userManager.GetUsersInRoleAsync("Employee");

            var numberOfUsers = Users.Count();

            return Ok(numberOfUsers);
        }

        [HttpGet("get-subsrciptions")]
        public async Task<IActionResult> GetAllSubsrciptions()
        {
            var subsrciptions = await _db.Subscribtions!.CountAsync();

            return Ok(subsrciptions);
        }

        [HttpGet("get-income")]
        public async Task<IActionResult> GetAllIncome()
        {
            decimal income = await _db.OrderItems!.Select(x => x.Price * x.Quantity).SumAsync();

            return Ok(income);
        }

        [HttpGet("get-recent-payments")]
        public async Task<IActionResult> GetRecentPayments()
        {
            var recentOrders = await _db.OrderItems!.Include(x => x.Order).Include(x => x.Product).Select(x => new RecentPayments
            {
                Id = x.Id,
                ProductName = x.Product.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                OrderId = x.OrderId,
                User = x.Order.CustomerName
            }).ToListAsync();

            return Ok(recentOrders);
        }

        [HttpGet("get-recent-users")]
        public async Task<IActionResult> GetRecentUsers()
        {
            var getAllRecentUsers = await _userManager.GetUsersInRoleAsync("User");
            var RecentUsers = getAllRecentUsers.Take(5);

            return Ok(RecentUsers);
        }

        [HttpGet("total-orders")]
        public async Task<IActionResult> TotalNumberOfOrders()
        {
            var totNrOfOrders = await _db.Orders!.CountAsync();
            return Ok(totNrOfOrders);
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _db.Products!.ToListAsync();
            return Ok(allProducts);
        }

        [HttpGet("orders-by-months")]
        public IActionResult GetOrdersByMonth()
        {
            var ordersByMonth = _db.Orders!
                .GroupBy(o => o.CreatedDate.Month)
                .Select(g => new OrderByMonthViewModel
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    NumberOfOrders = g.Count()
                }).ToList();


            return Ok(ordersByMonth);
        }

        [HttpGet("sales-by-month")]
        public async Task<ActionResult<IEnumerable<SaleByMonth>>> GetSalesByMonth()
        {
            var sales = await (from o in _db.Orders
                               join od in _db.OrderItems! on o.Id equals od.OrderId
                               where o.CreatedDate.Month == DateTime.Now.Month && o.CreatedDate.Year == DateTime.Now.Year
                               group od by new { o.CreatedDate.Year, o.CreatedDate.Month } into g
                               select new SaleByMonth
                               {
                                   Month = g.Key.Month,
                                   Year = g.Key.Year,
                                   TotalSales = g.Sum(od => od.Price * od.Quantity)
                               })
                              .ToListAsync();

                var lastMonthSales = await (from o in _db.Orders
                                            join od in _db.OrderItems! on o.Id equals od.OrderId
                                            where o.CreatedDate.Month == DateTime.Now.Month - 1 && o.CreatedDate.Year == DateTime.Now.Year
                                            group od by new { o.CreatedDate.Year, o.CreatedDate.Month } into g
                                            select new
                                            {
                                                TotalSales = g.Sum(od => od.Price * od.Quantity)
                                            })
                                           .SingleOrDefaultAsync();

                if (lastMonthSales != null && lastMonthSales.TotalSales > 0)
                {
                    var currentMonthSales = sales.SingleOrDefault();
                    var percentIncrease = ((currentMonthSales!.TotalSales - lastMonthSales.TotalSales) / lastMonthSales.TotalSales) * 100;
                    currentMonthSales.PercentIncrease = percentIncrease;
                }

            return Ok(sales);
        }


        [HttpGet("sales-by-months")]
        public IActionResult GetSalesByMonths()
        {

            var ordersByMonth = _db.Orders.Include(x => x.Details).ToList()
                .Where(o => o.CreatedDate.Year == 2023)
                .GroupBy(o => o.CreatedDate.Month)
                .Select(g => new SalesByMonthViewModel
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    TotalSales = g.Sum(o => o.Details!.Sum(od => od.Quantity * od.Price))
                })
                .OrderBy(g => g.Month)
                .ToList();

            return Ok(ordersByMonth);

        }
        #endregion

    }
}