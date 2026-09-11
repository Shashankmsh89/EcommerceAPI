using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<int> CreateAsync(
            int customerId,
            string tokenHash,
            DateTime expiresOn,
            CancellationToken cancellationToken);

        Task<RefreshToken?> GetAsync(
            string tokenHash,
            CancellationToken cancellationToken);

        Task RevokeAsync(
            string tokenHash,
            string? replacedByTokenHash,
            CancellationToken cancellationToken);
    }
}