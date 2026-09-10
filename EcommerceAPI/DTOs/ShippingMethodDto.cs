namespace EcommerceAPI.DTOs
{
    public class ShippingMethodDto
    {
        public int ShippingMethodId { get; set; }
        public string MethodName { get; set; }
        public decimal ShippingCharge { get; set; }
    }
}