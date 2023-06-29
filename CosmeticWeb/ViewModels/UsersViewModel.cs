using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.ViewModels
{
    public class UsersViewModel
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        [Display(Name = "New role")]
        public string? NewRole { get; set; }

        public string? Page { get; set; }
    }
}