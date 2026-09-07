namespace EcommerceAPI.DTOs
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ItemSubtotal { get; set; }
    }
}