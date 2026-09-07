namespace EcommerceAPI.Models
{
    public class ProductAvailability
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}