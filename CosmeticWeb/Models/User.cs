#region
using CosmeticWeb.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace CosmeticWeb.Models
{
    /// <summary>
    ///     Nje  klase  user qe  zgjeron 
    ///     funksionalitetet e <see cref="IdentityUser"/>.
    /// </summary>
    public class User : IdentityUser
    {
        #region Ditelindja userit
        /// <summary>
        ///     Ditelidja e userit.
        ///     Kjo perperty eshte required 
        ///     duke e dekoruar me  <see cref="RequiredAttribute"/>.
        /// </summary>
        [Required]
        public DateTime BirthDay { get; set; }
        #endregion

        #region Gjinia userit
        /// <summary>
        ///     Gjinia e userit. Kjo perperty eshte required 
        ///     duke e dekoruar me  <see cref="RequiredAttribute"/>.
        /// </summary>
        [Required]
        public string Gender { get; set; } = string.Empty;
        #endregion

        #region Pathi i imazhit te userit
        /// <summary>
        ///     Pathi i imazhit  te  userit
        /// </summary>
        public string? Image { get; set; }
        #endregion

        #region File i imazhit
        /// <summary>
        ///     File i imazhit.
        /// </summary>
        [NotMapped]
        [ImageFileValidation]
        [Required(ErrorMessage = "Image file is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion
    }
}