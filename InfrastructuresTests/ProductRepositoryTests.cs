// using DataAccess;
// using Domain.Entities;
// using Domain.Repositories;
// using Microsoft.EntityFrameworkCore;
// using NUnit.Framework;
// using Repositories;
//
// namespace InfrastructuresTests;
//
//
//      [TestFixture]
//     public class ProductRepositoryTests
//     {
//         private SnappFoodDbContext _dbContext;
//         private IProductRepository _productRepository;
//
//         [SetUp]
//         public void Setup()
//         {
//             var options = new DbContextOptionsBuilder<SnappFoodDbContext>()
//                 .UseInMemoryDatabase(databaseName: "SnappFoodTestDb")
//                 .Options;
//             _dbContext = new SnappFoodDbContext(options);
//             _productRepository = new ProductRepository(_dbContext);
//         }
//
//         [Test]
//         public async Task AddAsync_ValidProduct_AddsProductToDatabase()
//         {
//             // Arrange
//             var product = new Product { Title = "Sample Product", InventoryCount = 10, Price = 9.99m, Discount = 0.2 };
//
//             // Act
//             await _productRepository.AddAsync(product);
//             await _dbContext.SaveChangesAsync();
//
//             // Assert
//             var result = await _dbContext.Products.FindAsync(product.Id);
//             Assert.NotNull(result);
//             Assert.AreEqual(product.Title, result.Title);
//         }
//
//         [Test]
//         public async Task UpdateAsync_ValidProduct_UpdatesProductInDatabase()
//         {
//             // Arrange
//             var product = new Product { Id = 1, Title = "Sample Product", InventoryCount = 10, Price = 9.99m, Discount = 0.2 };
//             await _dbContext.Products.AddAsync(product);
//             await _dbContext.SaveChangesAsync();
//
//             // Act
//             product.Title = "Updated Product";
//             await _productRepository.UpdateAsync(product);
//             await _dbContext.SaveChangesAsync();
//
//             // Assert
//             var result = await _dbContext.Products.FindAsync(product.Id);
//             Assert.NotNull(result);
//             Assert.AreEqual(product.Title, result.Title);
//         }
//
//         [Test]
//         public async Task GetByIdAsync_ExistingId_ReturnsProduct()
//         {
//             // Arrange
//             var product = new Product { Id = 1, Title = "Sample Product", InventoryCount = 10, Price = 9.99m, Discount = 0.2 };
//             await _dbContext.Products.AddAsync(product);
//             await _dbContext.SaveChangesAsync();
//
//             // Act
//             var result = await _productRepository.GetByIdAsync(product.Id);
//
//             // Assert
//             Assert.NotNull(result);
//             Assert.AreEqual(product.Id, result.Id);
//         }
//
//         [Test]
//         public async Task GetByIdAsync_NonExistingId_ReturnsNull()
//         {
//             // Arrange
//             var nonExistingId = 999;
//
//             // Act
//             var result = await _productRepository.GetByIdAsync(nonExistingId);
//
//             // Assert
//             Assert.Null(result);
//         }
//     }
// }