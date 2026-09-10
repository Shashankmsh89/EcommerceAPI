using System.Data;
using EcommerceAPI.DTOs;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly IConfiguration _configuration;

        public OrderHistoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(
            int customerId,
            string? orderStatus,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var orders = new List<OrderHistoryDto>();

            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetOrderHistory",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            command.Parameters.AddWithValue(
                "@OrderStatus",
                string.IsNullOrWhiteSpace(orderStatus)
                    ? DBNull.Value
                    : orderStatus);

            command.Parameters.AddWithValue(
                "@Page",
                page);

            command.Parameters.AddWithValue(
                "@PageSize",
                pageSize);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                orders.Add(new OrderHistoryDto
                {
                    OrderId = reader.GetInt32(
                        reader.GetOrdinal("OrderId")),

                    CustomerId = reader.GetInt32(
                        reader.GetOrdinal("CustomerId")),

                    ShippingMethodId = reader.GetInt32(
                        reader.GetOrdinal("ShippingMethodId")),

                    ShippingMethodName = reader.GetString(
                        reader.GetOrdinal("ShippingMethodName")),

                    Subtotal = reader.GetDecimal(
                        reader.GetOrdinal("Subtotal")),

                    ShippingCharge = reader.GetDecimal(
                        reader.GetOrdinal("ShippingCharge")),

                    TaxAmount = reader.GetDecimal(
                        reader.GetOrdinal("TaxAmount")),

                    FinalTotal = reader.GetDecimal(
                        reader.GetOrdinal("FinalTotal")),

                    PaymentStatus = reader.GetString(
                        reader.GetOrdinal("PaymentStatus")),

                    PaymentTransactionReference =
                        reader.IsDBNull(
                            reader.GetOrdinal(
                                "PaymentTransactionReference"))
                            ? null
                            : reader.GetString(
                                reader.GetOrdinal(
                                    "PaymentTransactionReference")),

                    OrderStatus = reader.GetString(
                        reader.GetOrdinal("OrderStatus")),

                    CreatedOn = reader.GetDateTime(
                        reader.GetOrdinal("CreatedOn")),

                    TotalRecords = reader.GetInt32(
                        reader.GetOrdinal("TotalRecords"))
                });
            }

            return orders;
        }

        public async Task ReorderAsync(
            ReorderRequestDto request,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.Reorder",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                request.CustomerId);

            command.Parameters.AddWithValue(
                "@OrderId",
                request.OrderId);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteReaderAsync(cancellationToken);
        }
    }
}