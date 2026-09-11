using EcommerceAPI.DTOs;

namespace EcommerceAPI.Repositories
{
    public interface IInventoryRepository
    {
        Task UpdateInventoryAsync(
            UpdateInventoryDto request,
            CancellationToken cancellationToken);

        Task BulkUpdateAsync(
            IEnumerable<InventoryBulkDto> inventory,
            CancellationToken cancellationToken);
    }
}