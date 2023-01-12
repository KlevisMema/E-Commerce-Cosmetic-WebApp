namespace CosmeticWeb.ViewModels
{
    public class UsersSettingsViewModel
    {
        public const string SectionName = "Users";
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string[]? Roles { get; set; }
    }
}