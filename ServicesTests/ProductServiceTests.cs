// using Domain.Entities;
// using Domain.Repositories;
// using Moq;
// using NUnit.Framework;
// using Services;
//
// namespace Tests;
//
// [TestFixture]
// public class ProductServiceTests
// {
//     private Mock<IProductRepository> _productRepositoryMock;
//     private Mock<IUserRepository> _userRepositoryMock;
//     private ProductService _productService;
//
//     [SetUp]
//     public void Setup()
//     {
//         _productRepositoryMock = new Mock<IProductRepository>();
//         _userRepositoryMock = new Mock<IUserRepository>();
//         _productService = new ProductService(_productRepositoryMock.Object, _userRepositoryMock.Object);
//     }
//
//     [Test]
//     public async Task AddProductAsync_ValidProduct_CallsRepositoryAddAsync()
//     {
//         // Arrange
//         var product = new Product { Title = "Sample Product", InventoryCount = 10, Price = 9.99m, Discount = 0.2 };
//
//         // Act
//         await _productService.AddProductAsync(product);
//
//         // Assert
//         _productRepositoryMock.Verify(r => r.AddAsync(product), Times.Once);
//     }
//
//     [Test]
//     public async Task IncreaseInventoryAsync_ValidProduct_CallsRepositoryGetByIdAsyncAndUpdateAsync()
//     {
//         // Arrange
//         var product = new Product { Id = 1, InventoryCount = 10 };
//         _productRepositoryMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
//
//         // Act
//         await _productService.IncreaseInventoryAsync(product.Id, 5);
//
//         // Assert
//         _productRepositoryMock.Verify(r => r.GetByIdAsync(product.Id), Times.Once);
//         _productRepositoryMock.Verify(r => r.UpdateAsync(product), Times.Once);
//         Assert.AreEqual(15, product.InventoryCount);
//     }
//
//     [Test]
//     public async Task GetProductAsync_ValidProduct_ReturnsProductWithDiscountedPrice()
//     {
//         // Arrange
//         var product = new Product { Id = 1, Price = 10.0m, Discount = 0.2 };
//
//         _productRepositoryMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
//
//         // Act
//         var result = await _productService.GetProductAsync(product.Id);
//
//         // Assert
//         _productRepositoryMock.Verify(r => r.GetByIdAsync(product.Id), Times.Once);
//         Assert.AreEqual(8.0m, result.Price);
//     }
//
//     [Test]
//     public async Task BuyProductAsync_ValidProductAndUser_DecrementsInventoryAndAddsOrder()
//     {
//         // Arrange
//         var product = new Product { Id = 1, InventoryCount = 10 };
//         var user = new User { Id = 1 };
//         var order = new Order { Product = product, CreationDate = DateTime.Now, Buyer = user };
//
//         _productRepositoryMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
//         _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
//
//         // Act
//         await _productService.BuyProductAsync(product.Id, user.Id);
//
//         // Assert
//         _productRepositoryMock.Verify(r => r.GetByIdAsync(product.Id), Times.Once);
//         _userRepositoryMock.Verify(r => r.GetByIdAsync(user.Id), Times.Once);
//         _userRepositoryMock.Verify(r => r.AddOrderAsync(user, order), Times.Once);
//         _productRepositoryMock.Verify(r => r.UpdateAsync(product), Times.Once);
//         Assert.AreEqual(9, product.InventoryCount);
//     }
// }