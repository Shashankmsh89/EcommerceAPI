using System.Data;
using EcommerceAPI.DTOs;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace EcommerceAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<OrderDto?> GetOrderAsync(
            int orderId,
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetOrder",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@OrderId", orderId);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (!await reader.ReadAsync(cancellationToken))
            {
                return null;
            }

            return new OrderDto
            {
                OrderId = reader.GetInt32(
                    reader.GetOrdinal("OrderId")),

                CustomerId = reader.GetInt32(
                    reader.GetOrdinal("CustomerId")),

                ShippingMethodId = reader.GetInt32(
                    reader.GetOrdinal("ShippingMethodId")),

                ShippingMethodName = reader.GetString(
                    reader.GetOrdinal("ShippingMethodName")),

                ShippingName = reader.GetString(
                    reader.GetOrdinal("ShippingName")),

                ShippingAddress = reader.GetString(
                    reader.GetOrdinal("ShippingAddress")),

                ShippingCity = reader.GetString(
                    reader.GetOrdinal("ShippingCity")),

                ShippingState = reader.GetString(
                    reader.GetOrdinal("ShippingState")),

                ShippingPostalCode = reader.GetString(
                    reader.GetOrdinal("ShippingPostalCode")),

                ShippingPhone = reader.GetString(
                    reader.GetOrdinal("ShippingPhone")),

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
                    reader.GetOrdinal("CreatedOn"))
            };
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(
            int orderId,
            int customerId,
            CancellationToken cancellationToken)
        {
            var items = new List<OrderItemDto>();

            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetOrderItems",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@OrderId", orderId);
            command.Parameters.AddWithValue("@CustomerId", customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                items.Add(new OrderItemDto
                {
                    OrderItemId = reader.GetInt32(
                        reader.GetOrdinal("OrderItemId")),

                    OrderId = reader.GetInt32(
                        reader.GetOrdinal("OrderId")),

                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    Quantity = reader.GetInt32(
                        reader.GetOrdinal("Quantity")),

                    UnitPrice = reader.GetDecimal(
                        reader.GetOrdinal("UnitPrice")),

                    ItemSubtotal = reader.GetDecimal(
                        reader.GetOrdinal("ItemSubtotal"))
                });
            }

            return items;
        }
    }
}