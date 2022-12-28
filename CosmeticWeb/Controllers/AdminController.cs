using Microsoft.AspNetCore.Mvc;

namespace CosmeticWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
