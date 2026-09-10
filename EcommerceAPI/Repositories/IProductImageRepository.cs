using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories
{
    public interface IProductImageRepository
    {
        Task UploadAsync(
            int productId,
            string fileName,
            string contentType,
            long fileSize,
            byte[] imageData,
            CancellationToken cancellationToken);

        Task<ProductImage?> GetAsync(
            int productId,
            CancellationToken cancellationToken);
    }
}