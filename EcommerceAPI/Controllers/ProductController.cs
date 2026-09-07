using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductController(
            IProductRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            // Validate pagination
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(
                    "Page and pageSize must be greater than 0.");
            }

            // Get products from Repository
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

            // Convert Product -> ProductDto
            var result =
                _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(result);
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
                await _repository.CreateAsync(request,cancellationToken);

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

            return Ok(new
            {
                message = "Product deleted successfully."
            });
        }
    }
}