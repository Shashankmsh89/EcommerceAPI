using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.Extensions.Caching.Memory;
using EcommerceAPI.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        private readonly IProductCacheService _productCacheService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductController(
             IProductRepository repository,
             IMapper mapper,
             IMemoryCache cache,
            IProductCacheService productCacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
            _productCacheService = productCacheService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll(
    string? search,
    int? categoryId,
    int? brandId,
    decimal? minPrice,
    decimal? maxPrice,
    decimal? minRating,
    string? sortBy = "productName",
    string? sortOrder = "asc",
    int page = 1,
    int pageSize = 10,
    CancellationToken cancellationToken = default)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(
                    "Page and pageSize must be greater than 0.");
            }

            var cacheKey =
                $"products:{search}:{categoryId}:{brandId}:{minPrice}:{maxPrice}:{minRating}:{sortBy}:{sortOrder}:{page}:{pageSize}";

            if (_cache.TryGetValue(
                cacheKey,
                out IEnumerable<ProductDto>? cachedProducts))
            {
                return CreateProductResponse(cachedProducts);
            }

            var products = await _repository.GetAllAsync(
                search,
                categoryId,
                brandId,
                minPrice,
                maxPrice,
                minRating,
                sortBy,
                sortOrder,
                page,
                pageSize,
                cancellationToken);

            var result =
                _mapper.Map<IEnumerable<ProductDto>>(products);

            _cache.Set(
                cacheKey,
                result,
                TimeSpan.FromMinutes(5));

            _productCacheService.AddKey(cacheKey);

            return CreateProductResponse(result);
        }

        // GET: api/Product/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
             int id,
             CancellationToken cancellationToken = default)
        {
            var product =
                await _repository.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var productDto =
                _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
        ProductRequestDto request,
        CancellationToken cancellationToken = default)
        {
            int productId =
                await _repository.CreateAsync(request, cancellationToken);

            _productCacheService.Invalidate();

            return Ok(new
            {
                message = "Product created successfully.",
                productId = productId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    ProductRequestDto request,
    CancellationToken cancellationToken = default)
        {
            bool updated =
                await _repository.UpdateAsync(id, request, cancellationToken);

            if (!updated)
            {
                return NotFound("Product not found");
            }

            _productCacheService.Invalidate();

            return Ok(new
            {
                message = "Product updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            bool deleted =
                await _repository.DeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound("Product not found");
            }

            _productCacheService.Invalidate();

            return Ok(new
            {
                message = "Product deleted successfully."
            });

        }
        private IActionResult CreateProductResponse(
            IEnumerable<ProductDto> products)
        {
            var json = JsonSerializer.Serialize(products);

            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(json));

            var etag = $"\"{Convert.ToHexString(hash)}\"";

            if (Request.Headers["If-None-Match"] == etag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers["ETag"] = etag;

            return Ok(products);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate(
            IEnumerable<ProductBulkDto> products,
            CancellationToken cancellationToken = default)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("At least one product is required.");
            }

            var result = await _repository.BulkCreateAsync(
                products,
                cancellationToken);

            _productCacheService.Invalidate();

            return Ok(new
            {
                message = "Products created successfully.",
                products = result
            });
        }
    }

}