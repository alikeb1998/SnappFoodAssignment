using Domain.Entities;

namespace Domain.Repositories;

public interface ICacheProvider
{
    Task<T> GetOrSetAsync<T>(long key, Func<Task<T>> getItemCallback, TimeSpan expirationTime);
    Task UpdateProduct(Product product);
    Task CacheData();
    Task AddProduct(Product product);
}