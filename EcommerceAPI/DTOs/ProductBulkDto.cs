namespace EcommerceAPI.DTOs
{
    public class ProductBulkDto
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public decimal Rating { get; set; }
        public decimal Price { get; set; }
    }
}