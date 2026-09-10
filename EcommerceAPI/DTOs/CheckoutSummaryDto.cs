namespace EcommerceAPI.DTOs
{
    public class CheckoutSummaryDto
    {
        public int CustomerId { get; set; }
        public int CartId { get; set; }
        public int ShippingMethodId { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalTotal { get; set; }
    }
}