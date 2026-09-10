using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrderHistoryController : ControllerBase
    {
        private readonly IOrderHistoryRepository _repository;
        private readonly ILogger<OrderHistoryController> _logger;

        public OrderHistoryController(
            IOrderHistoryRepository repository,
            ILogger<OrderHistoryController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetHistory(
            int customerId,
            [FromQuery] string? orderStatus,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(
                    "Page and pageSize must be greater than 0.");
            }

            _logger.LogInformation(
                "Getting order history for CustomerId {CustomerId}",
                customerId);

            var orders = await _repository.GetOrderHistoryAsync(
                customerId,
                orderStatus,
                page,
                pageSize,
                cancellationToken);

            return Ok(orders);
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder(
            ReorderRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Reordering OrderId {OrderId} for CustomerId {CustomerId}",
                request.OrderId,
                request.CustomerId);

            await _repository.ReorderAsync(
                request,
                cancellationToken);

            return Ok(new
            {
                message = "Order items added to cart successfully."
            });
        }
    }
}