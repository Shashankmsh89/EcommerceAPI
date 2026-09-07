using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryRepository repository,
            ILogger<InventoryController> logger)
        {
            _repository = repository;
            _logger = logger;
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

            return Ok(new
            {
                message = "Inventory updated successfully."
            });
        }
    }
}