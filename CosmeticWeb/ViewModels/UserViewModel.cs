using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confrim password is not the same as password")]
        public string? ConfirmPassword { get; set; }

    }
}