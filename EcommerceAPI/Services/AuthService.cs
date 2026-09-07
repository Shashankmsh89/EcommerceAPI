using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            ICustomerRepository customerRepository,
            IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        public async Task<int> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(
                request.Password);

            return await _customerRepository.RegisterAsync(
                request,
                passwordHash,
                cancellationToken);
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

            if (customer == null)
            {
                return null;
            }

            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    customer.PasswordHash);

            if (!passwordValid)
            {
                return null;
            }

            string token = GenerateJwtToken(
                customer.CustomerId,
                customer.Email,
                customer.Role);

            return new LoginResponseDto
            {
                Token = token,
                CustomerId = customer.CustomerId,
                Email = customer.Email,
                Role = customer.Role
            };
        }

        private string GenerateJwtToken(
            int customerId,
            string email,
            string role)
        {
            var jwtKey = _configuration["Jwt:Key"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    customerId.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    email),

                new Claim(
                    ClaimTypes.Role,
                    role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}