using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(
            string? search,
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            decimal? minRating = null,
            string? sortBy = "productName",
            string? sortOrder = "asc",
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        Task<Product?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<int> CreateAsync(
            ProductRequestDto request,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            int id,
            ProductRequestDto request,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}