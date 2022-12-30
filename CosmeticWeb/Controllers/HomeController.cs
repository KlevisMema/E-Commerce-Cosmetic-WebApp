using CosmeticWeb.Data;
using CosmeticWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CosmeticWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            var newArrivals = await _context.Products.OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var firstHomeSlider =  await _context.homeSliders.Take(1).ToListAsync();
            var restHomeSlider =  await _context.homeSliders.Skip(1).ToListAsync();
            var testimonials =  await _context.Testimonials.ToListAsync();
            var blogs =  await _context.Blogs.ToListAsync();
            var galerieImages =  await _context.Galleries.ToListAsync();
            var teamMembers =  await _context.CosmeticTeamMembers.ToListAsync();

            ViewBag.Products = products;
            ViewBag.NewArrivals = newArrivals;
            ViewBag.Categories = categories;
            ViewBag.FirstHomeSlider = firstHomeSlider;
            ViewBag.RestHomeSliders = restHomeSlider;
            ViewBag.Testimonials = testimonials;
            ViewBag.Blogs = blogs;
            ViewBag.Gallery = galerieImages;
            ViewBag.TeamMembers = teamMembers;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}