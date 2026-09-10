using System.Data;
using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly IConfiguration _configuration;

        public ProductImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadAsync(
            int productId,
            string fileName,
            string contentType,
            long fileSize,
            byte[] imageData,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.UploadProductImage",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            command.Parameters.AddWithValue(
                "@FileName",
                fileName);

            command.Parameters.AddWithValue(
                "@ContentType",
                contentType);

            command.Parameters.AddWithValue(
                "@FileSize",
                fileSize);

            command.Parameters.Add(
                "@ImageData",
                SqlDbType.VarBinary,
                -1).Value = imageData;

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteReaderAsync(cancellationToken);
        }

        public async Task<ProductImage?> GetAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetProductImage",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            await connection.OpenAsync(cancellationToken);

            using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new ProductImage
                {
                    ProductImageId = reader.GetInt32(
                        reader.GetOrdinal("ProductImageId")),

                    ProductId = reader.GetInt32(
                        reader.GetOrdinal("ProductId")),

                    FileName = reader.GetString(
                        reader.GetOrdinal("FileName")),

                    ContentType = reader.GetString(
                        reader.GetOrdinal("ContentType")),

                    FileSize = reader.GetInt64(
                        reader.GetOrdinal("FileSize")),

                    ImageData = (byte[])reader["ImageData"],

                    UploadedOn = reader.GetDateTime(
                        reader.GetOrdinal("UploadedOn"))
                };
            }

            return null;
        }
    }
}