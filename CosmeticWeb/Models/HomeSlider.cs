using CosmeticWeb.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class HomeSlider
    {
        #region Id e home slider
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        #endregion

        #region Koment
        [Required(ErrorMessage = "Comment is required")]
        public string? Comment { get; set; }
        #endregion

        #region Lexo me shume
        [Required(ErrorMessage = "Read more is required")]
        [DisplayName("Read more")]
        public string? ReadMore {get; set; }
        #endregion

        #region Image dhe image file
        public string? Image { get; set; }

        [NotMapped]
        [ImageFileValidation]
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion
    }
}