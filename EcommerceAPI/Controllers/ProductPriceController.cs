using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPriceController : ControllerBase
    {
        private readonly IProductPriceRepository _repository;
        private readonly IMapper _mapper;

        public ProductPriceController(
            IProductPriceRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prices =
                await _repository.GetAllAsync();

            var result =
                _mapper.Map<IEnumerable<ProductPriceDto>>(prices);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var price =
                await _repository.GetByIdAsync(id);

            if (price == null)
            {
                return NotFound("Product price not found");
            }

            var result =
                _mapper.Map<ProductPriceDto>(price);

            return Ok(result);
        }
    }
}