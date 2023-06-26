using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICacheProvider _cacheProvider;

    public ProductService(IProductRepository productRepository, IUserRepository userRepository,IOrderRepository orderRepository,
        ICacheProvider cacheProvider)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
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
        _cacheProvider.UpdateProductAsync(product);
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<Product> GetProductAsync(long id)
    {
        var product = await _cacheProvider.GetOrSetAsync<Product>(id,
            async () => await _productRepository.GetByIdAsync(id),
            TimeSpan.FromDays(1));
        if (product == null)
            throw new NotFoundException("Product not found.");

        var discountedPrice = product.Price * (100 - (decimal)product.Discount)/100;
        product.Price = Math.Round(discountedPrice, 2);

        return product;
    }

    public async Task<bool> BuyProductAsync(long productId, long userId)
    {
        var product = await _cacheProvider.GetOrSetAsync<Product>(productId,
            async () => await _productRepository.GetByIdAsync(productId),
            TimeSpan.FromDays(1));

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
            ProductId = product.Id,
            CreationDate = DateTime.Now,
            Buyer = user,
            UserId = user.Id
        };

        await _orderRepository.AddOrderAsync(order);
        product.InventoryCount--;
        if (!await _productRepository.UpdateAsync(product)) return false;
        await _cacheProvider.UpdateProductAsync(product);
        return true;
    }

    public async Task<bool> CacheProducts()
    {
        var products = await _productRepository.Products();
        if (!products.Any()) return false;
        _cacheProvider.CacheProducts(products);
        return true;
    }
}