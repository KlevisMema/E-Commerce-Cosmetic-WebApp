namespace CosmeticWeb.Helpers
{
    public class ModeHelper
    {
        public static string GetModeClass(HttpRequest request)
        {
            var modePreference = request.Cookies["mode"];

            if (modePreference == "dark-mode")
            {
                return "dark-mode";
            }
            else if (modePreference == "bright-mode")
            {
                return "bright-mode";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}