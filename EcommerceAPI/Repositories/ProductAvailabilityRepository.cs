using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class ProductAvailabilityRepository : IProductAvailabilityRepository
    {
        private readonly string _connectionString;

        public ProductAvailabilityRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");
        }

        public async Task<ProductAvailability?> GetAvailabilityAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);

            using var command = new SqlCommand(
                "shashank.GetProductAvailability",
                connection);

            command.CommandType =
                System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new ProductAvailability
                {
                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    StockQuantity = reader.GetInt32(
                        reader.GetOrdinal("StockQuantity")),

                    IsAvailable = reader.GetBoolean(
                        reader.GetOrdinal("IsAvailable"))
                };
            }

            return null;
        }

        public async Task<IEnumerable<ProductAvailability>> ValidateInventoryAsync(
            string productIds,
            CancellationToken cancellationToken)
        {
            var products = new List<ProductAvailability>();

            using var connection =
                new SqlConnection(_connectionString);

            using var command = new SqlCommand(
                "shashank.ValidateInventory",
                connection);

            command.CommandType =
                System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductIds",
                productIds);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                products.Add(new ProductAvailability
                {
                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    StockQuantity = reader.GetInt32(
                        reader.GetOrdinal("StockQuantity")),

                    IsAvailable = reader.GetBoolean(
                        reader.GetOrdinal("IsAvailable"))
                });
            }

            return products;
        }
    }
}