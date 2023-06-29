using Microsoft.AspNetCore.Mvc;

namespace CosmeticWeb.Controllers
{
    public class ModeController : Controller
    {
        [HttpPost]
        public IActionResult SetModeNight(string mode, string url)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                SameSite = SameSiteMode.None,
                Secure = true,
            };

            Response.Cookies.Append("mode", mode, cookieOptions);

            return Redirect(url);
        }

        [HttpPost]
        public IActionResult SetModeBright(string mode, string url)
        {

            Response.Cookies.Append("mode", "light");

            return Redirect(url);
        }

        [HttpPost]
        public IActionResult SetModeNightWhenNotFound(string mode)
        {

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                SameSite = SameSiteMode.None,
                Secure = true,
            };

            Response.Cookies.Append("mode", mode, cookieOptions);

            return Ok();
        }

        [HttpPost]
        public IActionResult SetModeBrightWhenNotFound(string mode)
        {

            Response.Cookies.Append("mode", "light");

            return Ok();
        }

    }
}
