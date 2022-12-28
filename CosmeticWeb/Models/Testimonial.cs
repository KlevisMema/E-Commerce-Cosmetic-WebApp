using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class Testimonial
    {
        #region Id e testimonials/reviews
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Emri i personit 
        [Required(ErrorMessage = "Name is required")]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string? Name { get; set; }
        #endregion

        #region Vlersimi 
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5)]
        public int Rating { get; set; }
        #endregion

        #region Pershkrimi per vleresimin
        [Required(ErrorMessage = "Description is required")]
        [StringLength(maximumLength: 100, MinimumLength = 10)]
        public int Description { get; set; }
        #endregion

        #region Imazhi i personit qe ben review
        public string? Image { get; set; }
        #endregion

        #region File imazhi i personit qe ben review nuk ruhet ne database
        [NotMapped]
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Data kur ky testimonial u krijua
        public DateTime CreatedAt { get; set; }
        #endregion
    }
}