using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Services;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repository;

        private readonly IProductCacheService _productCacheService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryRepository repository,
            ILogger<InventoryController> logger,
            IProductCacheService productCacheService)
        {
            _repository = repository;
            _logger = logger;
            _productCacheService = productCacheService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventory(
            UpdateInventoryDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Updating inventory for ProductId {ProductId} to {StockQuantity}",
                request.ProductId,
                request.StockQuantity);

            await _repository.UpdateInventoryAsync(
                request,
                cancellationToken);

            _productCacheService.Invalidate();

            return Ok(new
            {
                message = "Inventory updated successfully."
            });
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> BulkUpdateInventory(
    IEnumerable<InventoryBulkDto> inventory,
    CancellationToken cancellationToken = default)
        {
            if (inventory == null || !inventory.Any())
            {
                return BadRequest("At least one inventory item is required.");
            }

            await _repository.BulkUpdateAsync(
                inventory,
                cancellationToken);

            return Ok(new
            {
                message = "Inventory updated successfully."
            });
        }
    }
}