using System.Data;
using Microsoft.Data.SqlClient;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _configuration;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> RegisterAsync(
            RegisterRequestDto request,
            string passwordHash,
            CancellationToken cancellationToken)
        {
            using SqlConnection connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command = new SqlCommand(
                "shashank.RegisterCustomer",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Email",
                request.Email);

            command.Parameters.AddWithValue(
                "@PasswordHash",
                passwordHash);

            command.Parameters.AddWithValue(
                "@Role",
                "Customer");

            await connection.OpenAsync(cancellationToken);

            var result = await command.ExecuteScalarAsync(
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<Customer?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            using SqlConnection connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command = new SqlCommand(
                "shashank.GetCustomerByEmail",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Email",
                email);

            await connection.OpenAsync(cancellationToken);

            using SqlDataReader reader =
                await command.ExecuteReaderAsync(
                    cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new Customer
            {
                CustomerId = reader.GetInt32(
                    reader.GetOrdinal("CustomerId")),

                Email = reader.GetString(
                    reader.GetOrdinal("Email")),

                PasswordHash = reader.GetString(
                    reader.GetOrdinal("PasswordHash")),

                Role = reader.GetString(
                    reader.GetOrdinal("Role")),

                CreatedOn = reader.GetDateTime(
                    reader.GetOrdinal("CreatedOn"))
            };
        }
    }
}