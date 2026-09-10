using EcommerceAPI.DTOs;

namespace EcommerceAPI.Repositories
{
    public interface ICheckoutRepository
    {
        Task<IEnumerable<ShippingMethodDto>> GetShippingMethodsAsync(
            CancellationToken cancellationToken);

        Task<CheckoutSummaryDto> GetCheckoutSummaryAsync(
            int customerId,
            int shippingMethodId,
            CancellationToken cancellationToken);

        Task<int> CreateOrderAsync(
            CreateOrderRequestDto request,
            CancellationToken cancellationToken);

        Task UpdatePaymentAsync(
           int orderId,
           string transactionReference,
           CancellationToken cancellationToken);
    }
}