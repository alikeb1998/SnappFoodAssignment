using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(long id);
}