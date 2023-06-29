using CosmeticWeb.Models;

namespace CosmeticWeb.ViewModels
{
    public class RecentPayments
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public string? ProductName { get; set; }
        public string? User { get; set; }
    }
}