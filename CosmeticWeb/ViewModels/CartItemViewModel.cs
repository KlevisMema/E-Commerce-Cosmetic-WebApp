using CosmeticWeb.Models;

namespace CosmeticWeb.ViewModels
{
    public class CartItemViewModel
    {
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCategory { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        public string? Image { get; set; }

        public CartItemViewModel()
        {
        }

        public CartItemViewModel(Product product, int quantity)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            ProductCategory = product.Category!.Name;
            ProductCategoryId = product.CategoryId;

            if (quantity == 0)
            {
                Quantity = 1;
            }
            else
            {
                Quantity = quantity;
            }

            Price = product.Price;
            Image = product.Image;
        }
    }
}