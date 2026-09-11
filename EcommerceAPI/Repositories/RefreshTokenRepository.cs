using System.Data;
using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IConfiguration _configuration;

        public RefreshTokenRepository(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateAsync(
            int customerId,
            string tokenHash,
            DateTime expiresOn,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.CreateRefreshToken",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustomerId",
                customerId);

            command.Parameters.AddWithValue(
                "@TokenHash",
                tokenHash);

            command.Parameters.AddWithValue(
                "@ExpiresOn",
                expiresOn);

            await connection.OpenAsync(cancellationToken);

            var result = await command.ExecuteScalarAsync(
                cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<RefreshToken?> GetAsync(
            string tokenHash,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.GetRefreshToken",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@TokenHash",
                tokenHash);

            await connection.OpenAsync(cancellationToken);

            using var reader = await command.ExecuteReaderAsync(
                cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return new RefreshToken
                {
                    RefreshTokenId = reader.GetInt32(
                        reader.GetOrdinal("RefreshTokenId")),

                    CustomerId = reader.GetInt32(
                        reader.GetOrdinal("CustomerId")),

                    TokenHash = reader.GetString(
                        reader.GetOrdinal("TokenHash")),

                    CreatedOn = reader.GetDateTime(
                        reader.GetOrdinal("CreatedOn")),

                    ExpiresOn = reader.GetDateTime(
                        reader.GetOrdinal("ExpiresOn")),

                    RevokedOn = reader.IsDBNull(
                        reader.GetOrdinal("RevokedOn"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("RevokedOn")),

                    ReplacedByTokenHash = reader.IsDBNull(
                        reader.GetOrdinal("ReplacedByTokenHash"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("ReplacedByTokenHash"))
                };
            }

            return null;
        }

        public async Task RevokeAsync(
            string tokenHash,
            string? replacedByTokenHash,
            CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            using var command = new SqlCommand(
                "shashank.RevokeRefreshToken",
                connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@TokenHash",
                tokenHash);

            command.Parameters.AddWithValue(
                "@ReplacedByTokenHash",
                (object?)replacedByTokenHash ?? DBNull.Value);

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteScalarAsync(
                cancellationToken);
        }
    }
}