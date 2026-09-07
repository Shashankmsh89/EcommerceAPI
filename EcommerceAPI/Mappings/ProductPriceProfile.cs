using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Mappings
{
    public class ProductPriceProfile : Profile
    {
        public ProductPriceProfile()
        {
            CreateMap<ProductPrice, ProductPriceDto>();
        }
    }
}