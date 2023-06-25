using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task AddOrderAsync(User user, Order order);   
}