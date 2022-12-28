using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class Product
    {
        #region Id e produktit
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri produktit
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(maximumLength: 15, MinimumLength = 3)]
        public string? Name { get; set; }
        #endregion

        #region Cmimi produktit
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Has to be decimal with two decimal points")]
        [Range(0, 2000, ErrorMessage = "The maximum possible value should be upto 5 digits")]
        [Required(ErrorMessage = "Product price is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        #endregion

        #region Cmimi i meparshem i produktit
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Has to be decimal with two decimal points")]
        [Range(0, 2000, ErrorMessage = "The maximum possible value should be upto 5 digits")]
        [Required(ErrorMessage = "Product previous price is required")]
        [Display(Name = " Previous price ")]
        [DataType(DataType.Currency)]
        public decimal PreviousPrice { get; set; }
        #endregion

        #region Pershkrimi per produktin
        [Required(ErrorMessage = "Product description is required")]
        [StringLength(maximumLength: 100, MinimumLength = 10)]
        public string? Description { get; set; }
        #endregion

        #region Rating i produktit
        [Required(ErrorMessage = "Rating is required")]
        [Display(Name = " Rating ")]
        [Range(1, 5)]
        public decimal Rating { get; set; }
        #endregion

        #region Data kur ky produkt eshte krijuar
        public DateTime CreatedAt { get; set; }
        #endregion

        #region Data kur ky produkt eshte modifikuar
        public DateTime ModifiedAt { get; set; }
        #endregion

        #region Pathi i imazhit te produktit
        public string? Image { get; set; }
        #endregion

        #region File i imazhit, nuk ruhet ne database
        [NotMapped]
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Lidhja e dy tabelave Produkt -> kategori me id e kategorise
        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        #endregion
    }
}