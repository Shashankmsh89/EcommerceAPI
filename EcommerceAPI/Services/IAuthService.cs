using EcommerceAPI.DTOs;

namespace EcommerceAPI.Services
{
    public interface IAuthService
    {
        Task<int> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken);

        Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken);

        Task<LoginResponseDto?> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken);
    }
}