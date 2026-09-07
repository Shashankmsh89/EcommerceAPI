using EcommerceAPI.DTOs;

namespace EcommerceAPI.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItemDto>> GetCartAsync(
            int customerId,
            CancellationToken cancellationToken);

        Task<decimal> GetCartSubtotalAsync(
            int customerId,
            CancellationToken cancellationToken);

        Task<int> GetCartItemCountAsync(
            int customerId,
            CancellationToken cancellationToken);

        Task AddToCartAsync(
            AddToCartDto request,
            CancellationToken cancellationToken);

        Task UpdateCartQuantityAsync(
            UpdateCartQuantityDto request,
            CancellationToken cancellationToken);

        Task RemoveFromCartAsync(
            int customerId,
            int productId,
            CancellationToken cancellationToken);

        Task ClearCartAsync(
            int customerId,
            CancellationToken cancellationToken);
    }
}