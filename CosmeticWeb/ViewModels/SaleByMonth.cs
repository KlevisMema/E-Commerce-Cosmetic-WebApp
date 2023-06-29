namespace CosmeticWeb.ViewModels
{
    public class SaleByMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalSales { get; set; }
        public decimal? PercentIncrease { get; set; }
    }
}
