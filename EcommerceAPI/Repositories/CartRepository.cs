using EcommerceAPI.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        // GET CART
        public async Task<IEnumerable<CartItemDto>> GetCartAsync(
            int customerId,
            CancellationToken cancellationToken)
        {
            var cartItems = new List<CartItemDto>();

            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand("shashank.GetCart", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                cartItems.Add(new CartItemDto
                {
                    CartId = reader.GetInt32(
                        reader.GetOrdinal("CartId")),

                    CustomerId = reader.GetInt32(
                        reader.GetOrdinal("CustomerId")),

                    CartItemId = reader.GetInt32(
                        reader.GetOrdinal("CartItemId")),

                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    Quantity = reader.GetInt32(
                        reader.GetOrdinal("Quantity")),

                    Price = reader.GetDecimal(
                        reader.GetOrdinal("Price")),

                    ItemSubtotal = reader.GetDecimal(
                        reader.GetOrdinal("ItemSubtotal"))
                });
            }

            return cartItems;
        }


        // GET CART SUBTOTAL
        public async Task<decimal> GetCartSubtotalAsync(
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand("shashank.GetCartSubtotal", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetDecimal(
                    reader.GetOrdinal("CartSubtotal"));
            }

            return 0;
        }


        // GET CART ITEM COUNT
        public async Task<int> GetCartItemCountAsync(
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand("shashank.GetCartItemCount", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetInt32(
                    reader.GetOrdinal("ItemCount"));
            }

            return 0;
        }


        // ADD TO CART
        public async Task AddToCartAsync(
            AddToCartDto request,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand("shashank.AddToCart", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                request.CustomerId);

            command.Parameters.AddWithValue(
                "@ProductId",
                request.ProductId);

            command.Parameters.AddWithValue(
                "@Quantity",
                request.Quantity);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }


        // UPDATE CART QUANTITY
        public async Task UpdateCartQuantityAsync(
            UpdateCartQuantityDto request,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.UpdateCartQuantity",
                    connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                request.CustomerId);

            command.Parameters.AddWithValue(
                "@ProductId",
                request.ProductId);

            command.Parameters.AddWithValue(
                "@Quantity",
                request.Quantity);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }


        // REMOVE FROM CART
        public async Task RemoveFromCartAsync(
            int customerId,
            int productId,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.RemoveFromCart",
                    connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }


        // CLEAR CART
        public async Task ClearCartAsync(
            int customerId,
            CancellationToken cancellationToken)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.ClearCart",
                    connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }
    }
}