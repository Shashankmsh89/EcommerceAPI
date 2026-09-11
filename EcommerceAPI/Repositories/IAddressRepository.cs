using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IAddressRepository
    {
        Task<int> CreateAsync(
            int customerId,
            AddressDto address,
            CancellationToken cancellationToken);

        Task<IEnumerable<Address>> GetByCustomerAsync(
            int customerId,
            CancellationToken cancellationToken);

        Task<Address?> GetByIdAsync(
            int addressId,
            int customerId,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            int addressId,
            int customerId,
            AddressDto address,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            int addressId,
            int customerId,
            CancellationToken cancellationToken);
    }
}