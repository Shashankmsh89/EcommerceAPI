using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartRepository repository,
            ILogger<CartController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/Cart/1
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(
            int customerId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Getting cart for CustomerId {CustomerId}",
                customerId);

            var cart = await _repository.GetCartAsync(
                customerId,
                cancellationToken);

            return Ok(cart);
        }


        // GET: api/Cart/1/subtotal
        [HttpGet("{customerId}/subtotal")]
        public async Task<IActionResult> GetSubtotal(
            int customerId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Getting cart subtotal for CustomerId {CustomerId}",
                customerId);

            var subtotal = await _repository.GetCartSubtotalAsync(
                customerId,
                cancellationToken);

            return Ok(new
            {
                customerId,
                subtotal
            });
        }


        // GET: api/Cart/1/count
        [HttpGet("{customerId}/count")]
        public async Task<IActionResult> GetItemCount(
            int customerId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Getting cart item count for CustomerId {CustomerId}",
                customerId);

            var count = await _repository.GetCartItemCountAsync(
                customerId,
                cancellationToken);

            return Ok(new
            {
                customerId,
                itemCount = count
            });
        }


        // POST: api/Cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(
            AddToCartDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Adding ProductId {ProductId} to CustomerId {CustomerId} cart",
                request.ProductId,
                request.CustomerId);

            await _repository.AddToCartAsync(
                request,
                cancellationToken);

            return Ok(new
            {
                message = "Product added to cart successfully."
            });
        }


        // PUT: api/Cart/1/3
        [HttpPut("{customerId}/{productId}")]
        public async Task<IActionResult> UpdateQuantity(
            int customerId,
            int productId,
            UpdateCartQuantityDto request,
            CancellationToken cancellationToken)
        {
            // Make sure route and body agree
            request.CustomerId = customerId;
            request.ProductId = productId;

            _logger.LogInformation(
                "Updating ProductId {ProductId} quantity for CustomerId {CustomerId}",
                productId,
                customerId);

            await _repository.UpdateCartQuantityAsync(
                request,
                cancellationToken);

            return Ok(new
            {
                message = "Cart quantity updated successfully."
            });
        }


        // DELETE: api/Cart/1/3
        [HttpDelete("{customerId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(
            int customerId,
            int productId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Removing ProductId {ProductId} from CustomerId {CustomerId} cart",
                productId,
                customerId);

            await _repository.RemoveFromCartAsync(
                customerId,
                productId,
                cancellationToken);

            return Ok(new
            {
                message = "Product removed from cart successfully."
            });
        }


        // DELETE: api/Cart/1/clear
        [HttpDelete("{customerId}/clear")]
        public async Task<IActionResult> ClearCart(
            int customerId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Clearing cart for CustomerId {CustomerId}",
                customerId);

            await _repository.ClearCartAsync(
                customerId,
                cancellationToken);

            return Ok(new
            {
                message = "Cart cleared successfully."
            });
        }
    }
} 