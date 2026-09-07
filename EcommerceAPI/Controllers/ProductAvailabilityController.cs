using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductAvailabilityController : ControllerBase
    {
        private readonly IProductAvailabilityRepository _repository;
        private readonly ILogger<ProductAvailabilityController> _logger;

        public ProductAvailabilityController(
            IProductAvailabilityRepository repository,
            ILogger<ProductAvailabilityController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/ProductAvailability/1
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAvailability(
            int productId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Checking availability for ProductId {ProductId}",
                productId);

            var result = await _repository.GetAvailabilityAsync(
                productId,
                cancellationToken);

            if (result == null)
            {
                _logger.LogWarning(
                    "Product {ProductId} was not found",
                    productId);

                return NotFound("Product not found");
            }

            return Ok(result);
        }

        // GET: api/ProductAvailability/validate?productIds=1,3,5,8
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateInventory(
            [FromQuery] string productIds,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(productIds))
            {
                return BadRequest("ProductIds are required.");
            }

            _logger.LogInformation(
                "Validating inventory for products: {ProductIds}",
                productIds);

            var result = await _repository.ValidateInventoryAsync(
                productIds,
                cancellationToken);

            return Ok(result);
        }
    }
}