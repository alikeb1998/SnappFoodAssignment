using Domain.Entities;

namespace Domain.Repositories;

public interface ICacheProvider
{
    Task<T> GetOrSetAsync<T>(long key, Func<Task<T>> getItemCallback, TimeSpan expirationTime);
    Task UpdateProductAsync(Product product);
    Task AddProduct(Product product);
    Task CacheProducts(List<Product> model);
}