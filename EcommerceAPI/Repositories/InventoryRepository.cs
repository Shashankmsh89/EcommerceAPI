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

        public async Task BulkUpdateAsync(
    IEnumerable<InventoryBulkDto> inventory,
    CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.BulkUpdateInventory",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            var table = new DataTable();

            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("StockQuantity", typeof(int));

            foreach (var item in inventory)
            {
                table.Rows.Add(
                    item.ProductId,
                    item.StockQuantity);
            }

            var parameter = command.Parameters.AddWithValue(
                "@Inventory",
                table);

            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = "shashank.InventoryBulkType";

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}