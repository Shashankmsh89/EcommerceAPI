using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<int> RegisterAsync(
            RegisterRequestDto request,
            string passwordHash,
            CancellationToken cancellationToken);

        Task<Customer?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken);

        Task<Customer?> GetByIdAsync(
            int customerId,
            CancellationToken cancellationToken);
    }
}