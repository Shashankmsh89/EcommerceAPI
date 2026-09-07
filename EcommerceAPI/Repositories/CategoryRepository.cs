using EcommerceAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection");
        }

        // GET ALL CATEGORIES
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = new List<Category>();

            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.GetCategories",
                    connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId =
                        reader.GetInt32(
                            reader.GetOrdinal("CategoryId")),

                    CategoryName =
                        reader.GetString(
                            reader.GetOrdinal("CategoryName"))
                });
            }

            return categories;
        }

        // GET CATEGORY BY ID
        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection =
                new SqlConnection(_connectionString);

            using var command =
                new SqlCommand(
                    "shashank.GetCategoryById",
                    connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CategoryId",
                id);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Category
                {
                    CategoryId =
                        reader.GetInt32(
                            reader.GetOrdinal("CategoryId")),

                    CategoryName =
                        reader.GetString(
                            reader.GetOrdinal("CategoryName"))
                };
            }

            return null;
        }
    }
}