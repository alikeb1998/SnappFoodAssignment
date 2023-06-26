using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepository
{
    Task<long> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    ValueTask<Product?> GetByIdAsync(long id);
    Task<List<Product>> Products();
}