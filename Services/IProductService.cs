using Domain.Dtos;
using Domain.Entities;

namespace Services;

public interface IProductService
{
    Task<bool> AddProductAsync(NewProductReq product);
    Task<bool> IncreaseInventoryAsync(long productId, long quantity);
    Task<Product> GetProductAsync(long id);
    Task<bool> BuyProductAsync(long productId, long userId);
    Task<bool> CacheProducts();
}