using DataAccess;
using Domain.Entities;
using Domain.Repositories;

namespace Repositories;

public class OrderRepository:IOrderRepository
{
    private readonly SnappFoodDbContext _dbContext;

    public OrderRepository(SnappFoodDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> AddOrderAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        return await _dbContext.SaveChangesAsync()>0;
    }
}