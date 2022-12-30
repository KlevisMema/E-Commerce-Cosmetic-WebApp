using System.ComponentModel.DataAnnotations;

namespace CosmeticWeb.Models
{
    public class OrderItem
    {
        #region Id e order item
        [Key]
        public Guid Id { get; set; }
        #endregion

        #region Cimimi
        public decimal Price { get; set; }
        #endregion

        #region Sasia
        public int Quantity { get; set; }
        #endregion

        #region Id e Orderit, lidhja me tabelen Order
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        #endregion

        #region Id e produktit, lidhja me tabelen e product
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        #endregion
    }
}