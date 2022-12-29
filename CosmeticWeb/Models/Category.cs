using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class Category
    {
        #region Id e Kategorise 
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri i kategorise
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 10, MinimumLength = 3)]
        public string? Name { get; set; }
        #endregion

        #region Data kur eshte krijuar kjo kategori
        public DateTime CreatedAt { get; set; }
        #endregion

        #region Data kur kjo kategori eshte modifikuar
        public DateTime ModifiedAt { get; set; }
        #endregion

        #region Pathi i imazhit te kategorise
        public string? Image { get; set; }
        #endregion

        #region File i imazhit, nuk ruhet ne database
        [NotMapped]
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Property qe ben lidhjen me tablen Product, lidhje 1:M
        public ICollection<Product>? Products { get; set; }
        #endregion
    }
}