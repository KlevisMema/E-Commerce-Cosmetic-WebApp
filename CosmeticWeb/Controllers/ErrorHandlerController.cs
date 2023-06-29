using Microsoft.AspNetCore.Mvc;

namespace CosmeticWeb.Controllers
{
    public class ErrorHandlerController : Controller
    {
        [Route("ErrorHandler/{statusCode}")]
        public IActionResult Index(int statusCode)
        {

            switch (statusCode)
            {
                case 404:
                    if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                    {
                        return View("NotFound_Admin");
                    }
                    else
                        return View("NotFound");

                default:
                    break;
            }

            return NoContent();

        }
    }
}