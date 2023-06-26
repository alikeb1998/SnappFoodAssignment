using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICacheProvider _cacheProvider;

    public ProductService(IProductRepository productRepository, IUserRepository userRepository,
        ICacheProvider cacheProvider)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _cacheProvider = cacheProvider;
    }

    public async Task<bool> AddProductAsync(NewProductReq product)
    {
        var newProduct = new Product()
        {
            Discount = product.Discount,
            InventoryCount = product.InventoryCount,
            Price = product.Price,
            Title = product.Title
        };
        var result = await _productRepository.AddAsync(newProduct);

        if (result is 0) return false;

        newProduct.Id = result;
        await _cacheProvider.AddProduct(newProduct);

        return true;
    }

    public async Task<bool> IncreaseInventoryAsync(long productId, long quantity)
    {
        var product = await _cacheProvider.GetOrSetAsync<Product>(productId,
            async () => await _productRepository.GetByIdAsync(productId),
            TimeSpan.FromDays(1));
        if (product == null)
            throw new NotFoundException("Product not found.");

        product.InventoryCount += quantity;
        _cacheProvider.UpdateProduct(product);
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<Product> GetProductAsync(long id)
    {
        var product = await _cacheProvider.GetOrSetAsync<Product>(id,
            async () => await _productRepository.GetByIdAsync(id),
            TimeSpan.FromDays(1));
        if (product == null)
            throw new NotFoundException("Product not found.");

        var discountedPrice = product.Price * (1 - (decimal)product.Discount);
        product.Price = Math.Round(discountedPrice, 2);

        return product;
    }

    public async Task BuyProductAsync(long productId, long userId)
    {
        var product = await _cacheProvider.GetOrSetAsync<Product>(productId,
            async () => await _productRepository.GetByIdAsync(productId),
            TimeSpan.FromDays(1));
        if (product == null)
            throw new NotFoundException("Product not found.");

        if (product.InventoryCount <= 0)
            throw new OutOfStockException("Product is out of stock.");

        var user = await _cacheProvider.GetOrSetAsync<User>(userId,
            async () => await _userRepository.GetByIdAsync(userId),
            TimeSpan.FromDays(1));
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
        _cacheProvider.UpdateProduct(product);
        await _productRepository.UpdateAsync(product);
    }
}