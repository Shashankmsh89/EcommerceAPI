using Asp.Versioning;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository repository,
            ILogger<OrderController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(
    int orderId,
    CancellationToken cancellationToken)
        {
            var customerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(customerIdClaim, out var customerId))
            {
                return Unauthorized();
            }

            _logger.LogInformation(
                "Getting OrderId {OrderId} for CustomerId {CustomerId}",
                orderId,
                customerId);

            var order = await _repository.GetOrderAsync(
                orderId,
                customerId,
                cancellationToken);

            if (order == null)
            {
                _logger.LogWarning(
                    "OrderId {OrderId} was not found for CustomerId {CustomerId}",
                    orderId,
                    customerId);

                return NotFound("Order not found");
            }

            return Ok(order);
        }

        [HttpGet("{orderId}/items")]
        public async Task<IActionResult> GetOrderItems(
    int orderId,
    CancellationToken cancellationToken)
        {
            var customerIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(customerIdClaim, out var customerId))
            {
                return Unauthorized();
            }

            _logger.LogInformation(
                "Getting items for OrderId {OrderId} for CustomerId {CustomerId}",
                orderId,
                customerId);

            var items = await _repository.GetOrderItemsAsync(
                orderId,
                customerId,
                cancellationToken);

            return Ok(items);
        }
    }
    
}
