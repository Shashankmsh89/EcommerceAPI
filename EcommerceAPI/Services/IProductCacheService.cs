namespace EcommerceAPI.Services
{
    public interface IProductCacheService
    {
        void AddKey(string cacheKey);

        void Invalidate();
    }
}