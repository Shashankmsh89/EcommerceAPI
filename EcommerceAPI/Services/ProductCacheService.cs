using Microsoft.Extensions.Caching.Memory;

namespace EcommerceAPI.Services
{
    public class ProductCacheService : IProductCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly HashSet<string> _cacheKeys = new();

        public ProductCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddKey(string cacheKey)
        {
            lock (_cacheKeys)
            {
                _cacheKeys.Add(cacheKey);
            }
        }

        public void Invalidate()
        {
            lock (_cacheKeys)
            {
                foreach (var key in _cacheKeys)
                {
                    _cache.Remove(key);
                }

                _cacheKeys.Clear();
            }
        }
    }
}