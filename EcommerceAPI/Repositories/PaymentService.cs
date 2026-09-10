namespace EcommerceAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        public Task<string> ProcessPaymentAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var transactionReference =
                Guid.NewGuid().ToString();

            _logger.LogInformation(
                "Mock payment processed successfully. TransactionReference: {TransactionReference}",
                transactionReference);

            return Task.FromResult(
                transactionReference);
        }
    }
}