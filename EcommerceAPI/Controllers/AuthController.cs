using EcommerceAPI.DTOs;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Registering customer with email {Email}",
                request.Email);

            var customerId = await _authService.RegisterAsync(
                request,
                cancellationToken);

            return Ok(new
            {
                customerId,
                message = "Customer registered successfully."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Login attempt for email {Email}",
                request.Email);

            var result = await _authService.LoginAsync(
                request,
                cancellationToken);

            if (result == null)
            {
                _logger.LogWarning(
                    "Invalid login attempt for email {Email}",
                    request.Email);

                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            return Ok(result);
        }
    }
}