using System.Data;
using EcommerceAPI.DTOs;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly IConfiguration _configuration;

        public CheckoutRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ShippingMethodDto>> GetShippingMethodsAsync(
            CancellationToken cancellationToken)
        {
            var methods = new List<ShippingMethodDto>();

            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetShippingMethods",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                methods.Add(new ShippingMethodDto
                {
                    ShippingMethodId = reader.GetInt32(
                        reader.GetOrdinal("ShippingMethodId")),

                    MethodName = reader.GetString(
                        reader.GetOrdinal("MethodName")),

                    ShippingCharge = reader.GetDecimal(
                        reader.GetOrdinal("ShippingCharge"))
                });
            }

            return methods;
        }

        public async Task<CheckoutSummaryDto> GetCheckoutSummaryAsync(
            int customerId,
            int shippingMethodId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetCheckoutSummary",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            command.Parameters.AddWithValue(
                "@ShippingMethodId",
                shippingMethodId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new CheckoutSummaryDto
                {
                    CustomerId = reader.GetInt32(
                        reader.GetOrdinal("CustomerId")),

                    CartId = reader.GetInt32(
                        reader.GetOrdinal("CartId")),

                    ShippingMethodId = reader.GetInt32(
                        reader.GetOrdinal("ShippingMethodId")),

                    ShippingCharge = reader.GetDecimal(
                        reader.GetOrdinal("ShippingCharge")),

                    Subtotal = reader.GetDecimal(
                        reader.GetOrdinal("Subtotal")),

                    TaxAmount = reader.GetDecimal(
                        reader.GetOrdinal("TaxAmount")),

                    FinalTotal = reader.GetDecimal(
                        reader.GetOrdinal("FinalTotal"))
                };
            }

            throw new Exception("Checkout summary could not be generated.");
        }

        public async Task<int> CreateOrderAsync(
            CreateOrderRequestDto request,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.CreateOrder",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                request.CustomerId);

            command.Parameters.AddWithValue(
                "@ShippingMethodId",
                request.ShippingMethodId);

            command.Parameters.AddWithValue(
                "@ShippingName",
                request.ShippingName);

            command.Parameters.AddWithValue(
                "@ShippingAddress",
                request.ShippingAddress);

            command.Parameters.AddWithValue(
                "@ShippingCity",
                request.ShippingCity);

            command.Parameters.AddWithValue(
                "@ShippingState",
                request.ShippingState);

            command.Parameters.AddWithValue(
                "@ShippingPostalCode",
                request.ShippingPostalCode);

            command.Parameters.AddWithValue(
                "@ShippingPhone",
                request.ShippingPhone);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetInt32(
                    reader.GetOrdinal("OrderId"));
            }

            throw new Exception("Order could not be created.");
        }

        public async Task UpdatePaymentAsync(
            int orderId,
            string transactionReference,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.UpdateOrderPayment",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@OrderId",
                orderId);

            command.Parameters.AddWithValue(
                "@TransactionReference",
                transactionReference);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteScalarAsync(cancellationToken);
        }
    }
    
}