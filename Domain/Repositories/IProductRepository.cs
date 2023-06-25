using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepository
{
    Task<bool> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    ValueTask<Product?> GetByIdAsync(long id);
}