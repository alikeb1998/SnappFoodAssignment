using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Moq;
using NUnit.Framework;
using Services;

namespace UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<ICacheProvider> _cacheProviderMock;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _cacheProviderMock = new Mock<ICacheProvider>();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                _userRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _cacheProviderMock.Object
            );
        }

        [Test]
        public async Task AddProductAsync_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var product = new NewProductReq
            {
                Title = "Product 1",
                InventoryCount = 10,
                Price = 9.99m,
                Discount = 0
            };

            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(1);

            _cacheProviderMock.Setup(cache => cache.AddProduct(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddProductAsync_FailedToAddProduct_ReturnsFalse()
        {
            // Arrange
            var product = new NewProductReq
            {
                Title = "Product 1",
                InventoryCount = 10,
                Price = 9.99m,
                Discount = 0
            };

            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(0);

            // Act
            var result = await _productService.AddProductAsync(product);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IncreaseInventoryAsync_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var productId = 1;
            var quantity = 5;
            var product = new Product
            {
                Id = productId,
                InventoryCount = 10
            };

            _cacheProviderMock.Setup(cache => cache.GetOrSetAsync<Product>(productId, It.IsAny<Func<Task<Product>>>(), TimeSpan.FromDays(1)))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.IncreaseInventoryAsync(productId, quantity);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetProductAsync_ValidId_ReturnsProduct()
        {
            long id = 4;
            var product = new Product
            {
                Id = id,
                InventoryCount = 10,
                Price = 100,
                Discount = 10,
                Title = "name"
            };
            _cacheProviderMock.Setup(cache => cache.GetOrSetAsync<Product>(id, It.IsAny<Func<Task<Product>>>(), TimeSpan.FromDays(1)))
                .ReturnsAsync(product);
            var result = await _productService.GetProductAsync(id);
            Assert.AreEqual(id, product.Id);
            Assert.AreEqual(product.Price,90);
        }

 

    }
}
