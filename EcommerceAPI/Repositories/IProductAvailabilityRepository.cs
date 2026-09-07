using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IProductAvailabilityRepository
    {
        Task<ProductAvailability?> GetAvailabilityAsync(
            int productId,
            CancellationToken cancellationToken);

        Task<IEnumerable<ProductAvailability>> ValidateInventoryAsync(
            string productIds,
            CancellationToken cancellationToken);
    }
}