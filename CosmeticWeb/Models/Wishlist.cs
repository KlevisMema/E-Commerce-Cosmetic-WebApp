using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class Wishlist
    {
        #region Id e wishlistes 
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region id e userit
        public Guid UserId { get; set; }
        #endregion

        #region Produkti 
        #region Pathi i imazhit te produktit
        public string? Image { get; set; }
        #endregion
        #region Emri produktit
        public string? Name { get; set; }
        #endregion
        #region Cmimi produktit
        public decimal Price { get; set; }
        #endregion
        #region Cmimi i meparshem i produktit
        public decimal PreviousPrice { get; set; }
        #endregion
        #region Id e produktit
        public Guid ProductId { get; set; }
        #endregion
        #endregion
    }
}