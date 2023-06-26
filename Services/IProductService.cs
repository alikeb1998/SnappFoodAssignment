using Domain.Dtos;
using Domain.Entities;

namespace Services;

public interface IProductService
{
    Task<bool> AddProductAsync(NewProductReq product);
    Task<bool> IncreaseInventoryAsync(long productId, long quantity);
    Task<Product> GetProductAsync(long id);
    Task BuyProductAsync(long productId, long userId);
}