using CosmeticWeb.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class Blog
    {
        #region Id e blog
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri i blogut
        [Required(ErrorMessage = "Blog name is required!")]
        [StringLength(maximumLength:20, MinimumLength = 4)]
        public string? Name { get; set; }
        #endregion

        #region Pershkrimi i blogut
        [StringLength(50)]
        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }
        #endregion

        #region Imazhi dhe imazh file
        public string? Image { get; set; }

        [NotMapped]
        [ImageFileValidation]
        [Required(ErrorMessage = "Image is required!")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Data kur ky rekord u krijua
        public DateTime? DateCreated { get; set; }
        #endregion

        #region Data kur ky rekord u modifikua
        public DateTime? DateModified { get; set; }
        #endregion

    }
}