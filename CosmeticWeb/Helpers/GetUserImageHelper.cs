using CosmeticWeb.Data;

namespace CosmeticWeb.Helpers
{
    public class GetUserImageHelper
    {
        public static string UserImagePath(string userEmail, ApplicationDbContext db)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return "";

            if (user.Image == null)
                return "";

            return user.Image;
        }
    }
}