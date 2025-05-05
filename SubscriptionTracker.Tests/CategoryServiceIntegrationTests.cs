using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Services;
using SubscriptionTracker.Domain.Entities;
using SubscriptionTracker.Infrastructure.Data;
using SubscriptionTracker.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SubscriptionTracker.Tests.Integration
{
    public class CategoryServiceIntegrationTests
    {
        private readonly SubscriptionAppContext _dbContext;
        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryService _categoryService;

        public CategoryServiceIntegrationTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<SubscriptionAppContext>()
                .UseInMemoryDatabase(databaseName: "SubscriptionTrackerTestDb")
                .Options;

            _dbContext = new SubscriptionAppContext(options);

            // Seed data
            SeedDatabase();

            // Initialize repository and service
            _categoryRepository = new CategoryRepository(_dbContext);
            _categoryService = new CategoryService(_categoryRepository);
        }

        private void SeedDatabase()
        {
            if (!_dbContext.Categories.AnyAsync().Result)
            {
                _dbContext.Categories.AddRange(new List<Category>
                {
                    new Category { Id = 1, Name = "Category1" },
                    new Category { Id = 2, Name = "Category2" }
                });
                _dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllCategories_ReturnsAllCategories()
        {
            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Category1", result[0].Name);
            Assert.Equal("Category2", result[1].Name);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory_WhenCategoryExists()
        {
            // Act
            var result = await _categoryService.GetCategoryById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Category1", result.Name);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNull_WhenCategoryDoesNotExist()
        {
            // Act
            var result = await _categoryService.GetCategoryById(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryByName_ReturnsCategory_WhenCategoryExists()
        {
            // Act
            var result = await _categoryService.GetCategoryByName("Category1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Category1", result.Name);
        }

        [Fact]
        public async Task GetCategoryByName_ReturnsNull_WhenCategoryDoesNotExist()
        {
            // Act
            var result = await _categoryService.GetCategoryByName("NonExistentCategory");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCategory_AddsNewCategory()
        {
            // Arrange
            var newCategory = new Category { Name = "NewCategory" };

            // Act
            var result = await _categoryService.CreateCategory(newCategory);

            // Assert
            Assert.True(result);

            var createdCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "NewCategory");
            Assert.NotNull(createdCategory);
            Assert.Equal("NewCategory", createdCategory.Name);
        }
    }
}
