using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class ProductPrice
    {
        public int ProductPriceId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}