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
        public string? Name { get; set; }
        #endregion

        #region Subjekti
        [Required(ErrorMessage = "Subject is required")]
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
        [Required(ErrorMessage = "Your message is required")]
        public string? Message { get; set; }
        #endregion
    }
}