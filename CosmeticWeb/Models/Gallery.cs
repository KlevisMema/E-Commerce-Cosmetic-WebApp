using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class Gallery
    {
        #region Id e fotos
        public Guid Id { get; set; }
        #endregion

        #region Pathi imazhit dhe file imazhi
        public string? Image { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Data kur u krijua
        public DateTime? DateCreated { get; set; }
        #endregion
    }
}