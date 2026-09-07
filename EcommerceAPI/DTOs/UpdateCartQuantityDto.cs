namespace EcommerceAPI.DTOs
{
    public class UpdateCartQuantityDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}