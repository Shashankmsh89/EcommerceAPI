using EcommerceAPI.DTOs;

namespace EcommerceAPI.Repositories
{
    public interface IOrderHistoryRepository
    {
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(
            int customerId,
            string? orderStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task ReorderAsync(
            ReorderRequestDto request,
            CancellationToken cancellationToken);
    }
}