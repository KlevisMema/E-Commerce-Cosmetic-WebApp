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

        #region Lista me produkte 
        public List<Product>? Products { get; set; }
        #endregion
    }
}