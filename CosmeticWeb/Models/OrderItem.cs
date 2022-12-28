using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class OrderItem
    {
        #region Id e order item
        [Key]
        public int Id { get; set; }
        #endregion

        #region Cimimi
        public decimal Price { get; set; }
        #endregion

        #region Sasia
        public int Quantity { get; set; }
        #endregion

        #region Id e Orderit, lidhja me tabelen Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        #endregion

        #region Id e produktit, lidhja me tabelen e product
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        #endregion
    }
}