using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class ContactUs
    {
        #region Id e na kontaktoni
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri i userit
        [Required(ErrorMessage = "Your name is required")]
        [StringLength(maximumLength:4, MinimumLength = 20)]
        public string? Name { get; set; }
        #endregion

        #region Subjekti
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(maximumLength: 10, MinimumLength = 150)]
        public string? Subject { get; set; }
        #endregion

        #region Data kur eshte krijuar/derguar ky kontakt
        public DateTime DateCreated { get; set; }
        #endregion

        #region Emaili i userit
        [Required(ErrorMessage = "Your email is required")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        #endregion

        #region Mesazhi 
        [StringLength(200, ErrorMessage = "You reached the limit of characters")]
        [Required(ErrorMessage = "Your message is required")]
        public string? Message { get; set; }
        #endregion
    }
}