using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IProductPriceRepository
    {
        Task<IEnumerable<ProductPrice>> GetAllAsync();
        Task<ProductPrice?> GetByIdAsync(int id);
    }
}