namespace EcommerceAPI.DTOs
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public int ShippingMethodId { get; set; }
        public string ShippingMethodName { get; set; }

        public decimal Subtotal { get; set; }
        public decimal ShippingCharge { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalTotal { get; set; }

        public string PaymentStatus { get; set; }
        public string PaymentTransactionReference { get; set; }
        public string OrderStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TotalRecords { get; set; }
    }
}