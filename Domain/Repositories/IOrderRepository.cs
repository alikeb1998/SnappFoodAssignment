using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<bool> AddOrderAsync(Order order);
}