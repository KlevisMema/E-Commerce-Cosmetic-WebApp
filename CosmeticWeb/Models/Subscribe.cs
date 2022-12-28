using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class Subscribe
    {
        #region id e Subscribe
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emaili i userit
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        #endregion

        #region Data kur ky user beri subscribe
        public DateTime SubscribedAt { get; set; }
        #endregion
    }
}