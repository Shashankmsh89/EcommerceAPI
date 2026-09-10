namespace EcommerceAPI.Services
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentAsync(
            CancellationToken cancellationToken);
    }
}