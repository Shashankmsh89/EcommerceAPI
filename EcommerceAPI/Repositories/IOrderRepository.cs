using EcommerceAPI.DTOs;

namespace EcommerceAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderDto?> GetOrderAsync(
            int orderId,
            int customerId,
            CancellationToken cancellationToken);

        Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(
            int orderId,
            int customerId,
            CancellationToken cancellationToken);
    }
}