using System.Security.Claims;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Customer")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _repository;
        private readonly ILogger<AddressController> _logger;

        public AddressController(
            IAddressRepository repository,
            ILogger<AddressController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(
            CancellationToken cancellationToken)
        {
            int customerId = GetCustomerId();

            _logger.LogInformation(
                "Getting addresses for CustomerId {CustomerId}",
                customerId);

            var addresses = await _repository.GetByCustomerAsync(
                customerId,
                cancellationToken);

            return Ok(addresses);
        }

        [HttpGet("{addressId:int}")]
        public async Task<IActionResult> GetAddress(
            int addressId,
            CancellationToken cancellationToken)
        {
            int customerId = GetCustomerId();

            var address = await _repository.GetByIdAsync(
                addressId,
                customerId,
                cancellationToken);

            if (address == null)
            {
                return NotFound("Address not found.");
            }

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(
            AddressDto request,
            CancellationToken cancellationToken)
        {
            int customerId = GetCustomerId();

            _logger.LogInformation(
                "Creating address for CustomerId {CustomerId}",
                customerId);

            int addressId = await _repository.CreateAsync(
                customerId,
                request,
                cancellationToken);

            return Ok(new
            {
                addressId,
                message = "Address created successfully."
            });
        }

        [HttpPut("{addressId:int}")]
        public async Task<IActionResult> UpdateAddress(
            int addressId,
            AddressDto request,
            CancellationToken cancellationToken)
        {
            int customerId = GetCustomerId();

            await _repository.UpdateAsync(
                addressId,
                customerId,
                request,
                cancellationToken);

            return Ok(new
            {
                message = "Address updated successfully."
            });
        }

        [HttpDelete("{addressId:int}")]
        public async Task<IActionResult> DeleteAddress(
            int addressId,
            CancellationToken cancellationToken)
        {
            int customerId = GetCustomerId();

            await _repository.DeleteAsync(
                addressId,
                customerId,
                cancellationToken);

            return Ok(new
            {
                message = "Address deleted successfully."
            });
        }

        private int GetCustomerId()
        {
            string? customerId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(customerId, out int id))
            {
                throw new UnauthorizedAccessException(
                    "Customer identity is missing from token.");
            }

            return id;
        }
    }
}