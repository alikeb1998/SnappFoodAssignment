using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Repositories;

public class InMemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _cache;
    /*private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;*/
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public InMemoryCacheProvider(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
    }

    public async Task<T> GetOrSetAsync<T>(long key, Func<Task<T>> getItemCallback, TimeSpan expirationTime)
    {
        if (_cache.TryGetValue(key, out T item))
        {
            return item;
        }

        item = await getItemCallback();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expirationTime);

        _cache.Set(key, item, cacheEntryOptions);

        return item;
    }

   
    public async Task UpdateProductAsync(Product product)
    {
       _cache.Remove(product.Id);
       _cache.Set(product.Id, product, _cacheEntryOptions);
    }

    public async Task AddProduct(Product product)
    {
        _cache.Set(product.Id, product, _cacheEntryOptions);
    }

    public async Task CacheProducts(List<Product> model)
    {
       model.ForEach(x=>_cache.Set(x.Id, x, _cacheEntryOptions));
    }

    public async Task Remove(string key)
    {
        _cache.Remove(key);
    }
}