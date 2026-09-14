namespace EcommerceAPI.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
    }
}