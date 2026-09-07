using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly string _connectionString;

        public ProductPriceRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        // GET ALL PRODUCT PRICES
        public async Task<IEnumerable<ProductPrice>> GetAllAsync()
        {
            var prices = new List<ProductPrice>();

            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand("shashank.GetProductPrices", connection);

            command.CommandType =
                System.Data.CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                prices.Add(new ProductPrice
                {
                    ProductPriceId =
                        reader.GetInt32(
                            reader.GetOrdinal("ProductPriceId")),

                    ProductId =
                        reader.GetInt32(
                            reader.GetOrdinal("ProductId")),

                    Price =
                        reader.GetDecimal(
                            reader.GetOrdinal("Price"))
                });
            }

            return prices;
        }

        // GET PRODUCT PRICE BY ID
        public async Task<ProductPrice?> GetByIdAsync(int id)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.GetProductPriceById",
                    connection);

            command.CommandType =
                System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductPriceId",
                id);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ProductPrice
                {
                    ProductPriceId =
                        reader.GetInt32(
                            reader.GetOrdinal("ProductPriceId")),

                    ProductId =
                        reader.GetInt32(
                            reader.GetOrdinal("ProductId")),

                    Price =
                        reader.GetDecimal(
                            reader.GetOrdinal("Price"))
                };
            }

            return null;
        }
    }
}