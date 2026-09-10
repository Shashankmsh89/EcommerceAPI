namespace EcommerceAPI.DTOs
{
    public class CreateOrderRequestDto
    {
        public int CustomerId { get; set; }
        public int ShippingMethodId { get; set; }
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostalCode { get; set; }
        public string ShippingPhone { get; set; }
    }
}