
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("database")]
        public async Task<IActionResult> TestDatabase()
        {
            string connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection =
                new SqlConnection(connectionString);

            await connection.OpenAsync();

            return Ok("Database connection successful!");
        }
    }
}