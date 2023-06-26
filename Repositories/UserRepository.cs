using DataAccess;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository : IUserRepository
{
    private readonly SnappFoodDbContext _dbContext;

    public UserRepository(SnappFoodDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User> GetByIdAsync(long id)
    {
        return _dbContext.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddOrderAsync(User user, Order order)
    {
        user.Orders.Add(order);
        _dbContext.Users.Add(user);
        _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> Users()
        => await _dbContext.Users.ToListAsync();
}