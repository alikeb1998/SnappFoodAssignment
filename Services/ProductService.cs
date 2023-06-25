using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public ProductService(IProductRepository productRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> AddProductAsync(NewProductReq product)
    {
        return await _productRepository.AddAsync(new()
        {
            Discount = product.Discount,
            InventoryCount = product.InventoryCount,
            Price = product.Price,
            Title = product.Title
        });
    }

    public async Task<bool> IncreaseInventoryAsync(long productId, long quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException("Product not found.");

        product.InventoryCount += quantity;
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<Product> GetProductAsync(long id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product not found.");

        var discountedPrice = product.Price * (1 - (decimal)product.Discount);
        product.Price = Math.Round(discountedPrice, 2);

        return product;
    }

    public async Task BuyProductAsync(int productId, int userId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException("Product not found.");

        if (product.InventoryCount <= 0)
            throw new OutOfStockException("Product is out of stock.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var order = new Order
        {
            Product = product,
            CreationDate = DateTime.Now,
            Buyer = user
        };

        await _userRepository.AddOrderAsync(user, order);
        product.InventoryCount--;
        await _productRepository.UpdateAsync(product);
    }
}