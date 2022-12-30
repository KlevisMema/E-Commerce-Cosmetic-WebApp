﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticWeb.Models
{
    public class CosmeticTeamMember
    {
        #region Id e persoit te skuadres
        public Guid Id { get; set; }
        #endregion

        #region Emri
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        #endregion

        #region Roli i personit ne skuader
        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }
        #endregion

        #region rrjetet sociale 
        public string? Linkedin { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        #endregion

        #region Pathi imazhit dhe file imazhi
        public string? Image { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile? ImageFile { get; set; }
        #endregion

        #region Data kur u krijua
        [DisplayName("Date created")]
        public DateTime? DateCreated { get; set; }
        #endregion
    }
}