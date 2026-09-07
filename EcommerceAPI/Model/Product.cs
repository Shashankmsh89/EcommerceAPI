using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BrandId { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
    }
}