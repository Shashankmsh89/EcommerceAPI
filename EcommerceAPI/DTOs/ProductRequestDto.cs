namespace EcommerceAPI.DTOs
{
    public class ProductRequestDto
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public decimal Rating { get; set; }
    }
}