using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(long id);
    Task AddOrderAsync(User user, Order order);
    Task<List<User>> Users();
}