using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }
    }
}