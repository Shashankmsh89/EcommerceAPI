using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Product>> GetAllAsync(
            string? search,
            int? categoryId = null,
            int? brandId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            decimal? minRating = null,
            string? sortBy = "productName",
            string? sortOrder = "asc",
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var products = new List<Product>();

            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.GetProducts",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Search",
                string.IsNullOrEmpty(search)
                    ? DBNull.Value
                    : search);

            command.Parameters.AddWithValue(
                "@CategoryId",
                categoryId ?? (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "@BrandId",
                brandId ?? (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "@MinPrice",
                minPrice ?? (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "@MaxPrice",
                maxPrice ?? (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "@MinRating",
                minRating ?? (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "@SortBy",
                string.IsNullOrEmpty(sortBy)
                    ? "productName"
                    : sortBy);

            command.Parameters.AddWithValue(
                "@SortOrder",
                string.IsNullOrEmpty(sortOrder)
                    ? "asc"
                    : sortOrder);

            command.Parameters.AddWithValue(
                "@Page",
                page);

            command.Parameters.AddWithValue(
                "@PageSize",
                pageSize);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken);

            while (await reader.ReadAsync(
                cancellationToken))
            {
                products.Add(new Product
                {
                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    CategoryId = reader.GetInt32(
                        reader.GetOrdinal("CategoryId")),

                    BrandId = reader.GetInt32(
                        reader.GetOrdinal("BrandId")),

                    Rating = reader.GetDecimal(
                        reader.GetOrdinal("Rating")),

                    CategoryName = reader.GetString(
                        reader.GetOrdinal("CategoryName")),

                    BrandName = reader.GetString(
                        reader.GetOrdinal("BrandName")),

                    Price = reader.GetDecimal(
                        reader.GetOrdinal("Price"))
                });
            }

            return products;
        }

        public async Task<Product?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.GetProductById",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                id);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(
                    cancellationToken);

            if (await reader.ReadAsync(
                cancellationToken))
            {
                return new Product
                {
                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    ProductName = reader.GetString(
                        reader.GetOrdinal("ProductName")),

                    CategoryId = reader.GetInt32(
                        reader.GetOrdinal("CategoryId")),

                    BrandId = reader.GetInt32(
                        reader.GetOrdinal("BrandId")),

                    Rating = reader.GetDecimal(
                        reader.GetOrdinal("Rating")),

                    CategoryName = reader.GetString(
                        reader.GetOrdinal("CategoryName")),

                    BrandName = reader.GetString(
                        reader.GetOrdinal("BrandName")),

                    Price = reader.GetDecimal(
                        reader.GetOrdinal("Price"))
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(
            ProductRequestDto request,
            CancellationToken cancellationToken = default)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.CreateProduct",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductName",
                request.ProductName);

            command.Parameters.AddWithValue(
                "@CategoryId",
                request.CategoryId);

            command.Parameters.AddWithValue(
                "@BrandId",
                request.BrandId);

            command.Parameters.AddWithValue(
                "@Rating",
                request.Rating);

            command.Parameters.AddWithValue(
                "@Price",
                request.Price);

            await connection.OpenAsync(cancellationToken);

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(
            int id,
            ProductRequestDto request,
            CancellationToken cancellationToken = default)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.UpdateProduct",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                id);

            command.Parameters.AddWithValue(
                "@ProductName",
                request.ProductName);

            command.Parameters.AddWithValue(
                "@CategoryId",
                request.CategoryId);

            command.Parameters.AddWithValue(
                "@BrandId",
                request.BrandId);

            command.Parameters.AddWithValue(
                "@Rating",
                request.Rating);

            command.Parameters.AddWithValue(
                "@Price",
                request.Price);

            await connection.OpenAsync(cancellationToken);

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result) == 1;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.DeleteProduct",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                id);

            await connection.OpenAsync(cancellationToken);

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result) == 1;
        }
    }
}