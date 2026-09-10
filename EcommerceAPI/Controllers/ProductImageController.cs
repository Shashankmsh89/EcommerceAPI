using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/Product")]
    [Authorize(Roles = "Admin")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageRepository _repository;
        private readonly ILogger<ProductImageController> _logger;

        public ProductImageController(
            IProductImageRepository repository,
            ILogger<ProductImageController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("{productId}/image")]
        public async Task<IActionResult> UploadImage(
            int productId,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
            {
                return BadRequest(
                    "Image size cannot exceed 5 MB.");
            }

            var extension = Path.GetExtension(file.FileName)
    .ToLowerInvariant();

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => null
            };

            if (contentType == null)
            {
                return BadRequest(
                    "Only JPEG, PNG and WEBP images are supported.");
            }

            _logger.LogInformation(
                "Uploading image for ProductId {ProductId}",
                productId);

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(
                memoryStream,
                cancellationToken);

            await _repository.UploadAsync(
                productId,
                file.FileName,
                contentType,
                file.Length,
                memoryStream.ToArray(),
                cancellationToken);

            return Ok(new
            {
                message = "Product image uploaded successfully.",
                productId,
                fileName = file.FileName,
                fileSize = file.Length
            });
        }

        [AllowAnonymous]
        [HttpGet("{productId}/image")]
        public async Task<IActionResult> DownloadImage(
            int productId,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Downloading image for ProductId {ProductId}",
                productId);

            var image = await _repository.GetAsync(
                productId,
                cancellationToken);

            if (image == null)
            {
                return NotFound("Product image not found.");
            }

            return File(
                image.ImageData,
                image.ContentType,
                image.FileName);
        }
    }
}