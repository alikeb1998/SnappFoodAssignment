using DataAccess;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SnappFoodDbContext _dbContext;

    public ProductRepository(SnappFoodDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddAsync(Product product)
    {
        if (product.Title.Length > 40)
            throw new BadRequestException("Product title must be less than 40 characters.");

        if (_dbContext.Products.Any(p => p.Title == product.Title))
            throw new BadRequestException("Product title must be unique.");

        _dbContext.Products.Add(product);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public ValueTask<Product?> GetByIdAsync(long id)
    {
        return _dbContext.Products.FindAsync(id);
    }
}