using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryController(
            ICategoryRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories =
                await _repository.GetAllAsync();

            var result =
                _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category =
                await _repository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            var result =
                _mapper.Map<CategoryDto>(category);

            return Ok(result);
        }
    }
}