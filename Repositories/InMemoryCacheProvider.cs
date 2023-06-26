using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Repositories;

public class InMemoryCacheProvider : ICacheProvider
{
    private readonly MemoryCache _cache;
    private readonly ProductRepository _productRepository;
    private readonly UserRepository _userRepository;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public InMemoryCacheProvider(ProductRepository productRepository, UserRepository userRepository)
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _productRepository = productRepository;
        _userRepository = userRepository;
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

   
    public async Task UpdateProduct(Product product)
    {
       _cache.Remove(product.Id);
       _cache.Set(product.Id, product, _cacheEntryOptions);
    }

    public async Task CacheData()
    {
        var products = await _productRepository.Products();
        var users = await _userRepository.Users();
        
        products.ForEach(x => _cache.Set(x.Id, x, _cacheEntryOptions));
        users.ForEach(x => _cache.Set(x.Id, x, _cacheEntryOptions));
    }

    public async Task AddProduct(Product product)
    {
        _cache.Set(product.Id, product, _cacheEntryOptions);
    }
    
}