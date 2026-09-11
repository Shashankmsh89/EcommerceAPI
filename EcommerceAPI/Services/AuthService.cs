using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            ICustomerRepository customerRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _refreshTokenRepository = refreshTokenRepository;
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

            DateTime accessTokenExpiresOn =
                DateTime.UtcNow.AddHours(2);

            string token = GenerateJwtToken(
                customer.CustomerId,
                customer.Email,
                customer.Role,
                accessTokenExpiresOn);

            string refreshToken = GenerateRefreshToken();

            string refreshTokenHash =
                HashRefreshToken(refreshToken);

            DateTime refreshTokenExpiresOn =
                DateTime.UtcNow.AddDays(7);

            await _refreshTokenRepository.CreateAsync(
                customer.CustomerId,
                refreshTokenHash,
                refreshTokenExpiresOn,
                cancellationToken);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresOn = accessTokenExpiresOn,
                CustomerId = customer.CustomerId,
                Email = customer.Email,
                Role = customer.Role
            };
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(
    string refreshToken,
    CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }

            string tokenHash = HashRefreshToken(refreshToken);

            var storedToken = await _refreshTokenRepository.GetAsync(
                tokenHash,
                cancellationToken);

            if (storedToken == null)
            {
                return null;
            }

            if (storedToken.RevokedOn.HasValue)
            {
                return null;
            }

            if (storedToken.ExpiresOn <= DateTime.UtcNow)
            {
                return null;
            }

            var customer = await _customerRepository.GetByIdAsync(
                storedToken.CustomerId,
                cancellationToken);

            if (customer == null)
            {
                return null;
            }

            DateTime accessTokenExpiresOn =
                DateTime.UtcNow.AddHours(2);

            string newAccessToken = GenerateJwtToken(
                customer.CustomerId,
                customer.Email,
                customer.Role,
                accessTokenExpiresOn);

            string newRefreshToken = GenerateRefreshToken();

            string newRefreshTokenHash =
                HashRefreshToken(newRefreshToken);

            DateTime newRefreshTokenExpiresOn =
                DateTime.UtcNow.AddDays(7);

            await _refreshTokenRepository.CreateAsync(
                customer.CustomerId,
                newRefreshTokenHash,
                newRefreshTokenExpiresOn,
                cancellationToken);

            await _refreshTokenRepository.RevokeAsync(
                tokenHash,
                newRefreshTokenHash,
                cancellationToken);

            return new LoginResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresOn = accessTokenExpiresOn,
                CustomerId = customer.CustomerId,
                Email = customer.Email,
                Role = customer.Role
            };
        }

        private string GenerateJwtToken(
            int customerId,
            string email,
            string role,
            DateTime expiresOn)
        {
            var jwtKey = _configuration["Jwt:Key"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            bool canManageProducts =
                role.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase);

            bool canManageOrders =
                role.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase);

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
                    role),

                new Claim(
                    "CanManageProducts",
                    canManageProducts.ToString()),

                new Claim(
                    "CanManageOrders",
                    canManageOrders.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiresOn,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        private static string HashRefreshToken(
            string refreshToken)
        {
            byte[] bytes =
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(refreshToken));

            return Convert.ToHexString(bytes);
        }
    }
}