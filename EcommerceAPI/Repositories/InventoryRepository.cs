using System.Data;
using EcommerceAPI.DTOs;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IConfiguration _configuration;

        public InventoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UpdateInventoryAsync(
            UpdateInventoryDto request,
            CancellationToken cancellationToken)
        {
            using SqlConnection connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using SqlCommand command = new SqlCommand(
                "shashank.UpdateProductInventory",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                request.ProductId);

            command.Parameters.AddWithValue(
                "@StockQuantity",
                request.StockQuantity);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteReaderAsync(cancellationToken);
        }
    }
}