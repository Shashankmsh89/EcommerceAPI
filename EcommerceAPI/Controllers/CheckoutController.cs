using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Services;
using Asp.Versioning;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutRepository _repository;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(
          ICheckoutRepository repository,
          IPaymentService paymentService,
          ILogger<CheckoutController> logger)
        {
            _repository = repository;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpGet("shipping-methods")]
        public async Task<IActionResult> GetShippingMethods(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Getting available shipping methods");

            var methods =
                await _repository.GetShippingMethodsAsync(
                    cancellationToken);

            return Ok(methods);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCheckoutSummary(
            int customerId,
            [FromQuery] int shippingMethodId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Getting checkout summary for CustomerId {CustomerId}",
                customerId);

            var summary =
                await _repository.GetCheckoutSummaryAsync(
                    customerId,
                    shippingMethodId,
                    cancellationToken);

            return Ok(summary);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(
    CreateOrderRequestDto request,
    CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating order for CustomerId {CustomerId}",
                request.CustomerId);

            var orderId =
                await _repository.CreateOrderAsync(
                    request,
                    cancellationToken);

            var transactionReference =
                await _paymentService.ProcessPaymentAsync(
                    cancellationToken);

            await _repository.UpdatePaymentAsync(
                orderId,
                transactionReference,
                cancellationToken);

            _logger.LogInformation(
                "Payment completed for OrderId {OrderId} with TransactionReference {TransactionReference}",
                orderId,
                transactionReference);

            return Ok(new
            {
                orderId,
                paymentStatus = "Paid",
                transactionReference,
                message = "Order created and payment processed successfully."
            });
        }
    }
}