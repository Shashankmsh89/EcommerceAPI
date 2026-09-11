using System.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IConfiguration _configuration;

        public AddressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(
            int customerId,
            AddressDto address,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.CreateAddress",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerId", customerId);
            command.Parameters.AddWithValue("@AddressType", address.AddressType);
            command.Parameters.AddWithValue("@RecipientName", address.RecipientName);
            command.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
            command.Parameters.AddWithValue(
                "@AddressLine2",
                (object?)address.AddressLine2 ?? DBNull.Value);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            command.Parameters.AddWithValue("@Country", address.Country);
            command.Parameters.AddWithValue("@Phone", address.Phone);
            command.Parameters.AddWithValue("@IsDefault", address.IsDefault);

            await connection.OpenAsync(cancellationToken);

            var result = await command.ExecuteScalarAsync(
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<Address>> GetByCustomerAsync(
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetCustomerAddresses",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader = await command.ExecuteReaderAsync(
                cancellationToken);

            var addresses = new List<Address>();

            while (await reader.ReadAsync(cancellationToken))
            {
                addresses.Add(MapAddress(reader));
            }

            return addresses;
        }

        public async Task<Address?> GetByIdAsync(
            int addressId,
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetAddressById",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AddressId", addressId);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader = await command.ExecuteReaderAsync(
                cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return MapAddress(reader);
            }

            return null;
        }

        public async Task UpdateAsync(
            int addressId,
            int customerId,
            AddressDto address,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.UpdateAddress",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AddressId", addressId);
            command.Parameters.AddWithValue("@CustomerId", customerId);
            command.Parameters.AddWithValue("@AddressType", address.AddressType);
            command.Parameters.AddWithValue("@RecipientName", address.RecipientName);
            command.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
            command.Parameters.AddWithValue(
                "@AddressLine2",
                (object?)address.AddressLine2 ?? DBNull.Value);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            command.Parameters.AddWithValue("@Country", address.Country);
            command.Parameters.AddWithValue("@Phone", address.Phone);
            command.Parameters.AddWithValue("@IsDefault", address.IsDefault);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteScalarAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            int addressId,
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.DeleteAddress",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AddressId", addressId);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteScalarAsync(cancellationToken);
        }

        private static Address MapAddress(SqlDataReader reader)
        {
            return new Address
            {
                AddressId = reader.GetInt32(
                    reader.GetOrdinal("AddressId")),

                CustomerId = reader.GetInt32(
                    reader.GetOrdinal("CustomerId")),

                AddressType = reader.GetString(
                    reader.GetOrdinal("AddressType")),

                RecipientName = reader.GetString(
                    reader.GetOrdinal("RecipientName")),

                AddressLine1 = reader.GetString(
                    reader.GetOrdinal("AddressLine1")),

                AddressLine2 = reader.IsDBNull(
                    reader.GetOrdinal("AddressLine2"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("AddressLine2")),

                City = reader.GetString(
                    reader.GetOrdinal("City")),

                State = reader.GetString(
                    reader.GetOrdinal("State")),

                PostalCode = reader.GetString(
                    reader.GetOrdinal("PostalCode")),

                Country = reader.GetString(
                    reader.GetOrdinal("Country")),

                Phone = reader.GetString(
                    reader.GetOrdinal("Phone")),

                IsDefault = reader.GetBoolean(
                    reader.GetOrdinal("IsDefault")),

                CreatedOn = reader.GetDateTime(
                    reader.GetOrdinal("CreatedOn"))
            };
        }
    }
}