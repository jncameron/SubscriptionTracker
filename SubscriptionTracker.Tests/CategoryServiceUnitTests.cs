using Moq;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces.Services;
using SubscriptionTracker.Application.Services;
using SubscriptionTracker.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SubscriptionTracker.Tests
{
    public class CategoryServiceUnitTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ICategoryService _categoryService;

        public CategoryServiceUnitTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsListOfCategoryDTOs()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Category1", result[0].Name);
            Assert.Equal("Category2", result[1].Name);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategoryDTO_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);

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
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByIdAsync(1))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await _categoryService.GetCategoryById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCategoryByName_ReturnsCategoryDTO_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1" };
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByNameAsync("Category1"))
                .ReturnsAsync(category);

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
            // Arrange
            _mockCategoryRepository.Setup(repo => repo.GetCategoryByNameAsync("Category1"))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await _categoryService.GetCategoryByName("Category1");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsTrue_WhenCategoryIsCreatedSuccessfully()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "NewCategory" };
            _mockCategoryRepository.Setup(repo => repo.CreateCategory(category))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.CreateCategory(category);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsFalse_WhenCategoryCreationFails()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "NewCategory" };
            _mockCategoryRepository.Setup(repo => repo.CreateCategory(category))
                .ReturnsAsync(false);

            // Act
            var result = await _categoryService.CreateCategory(category);

            // Assert
            Assert.False(result);
        }
    }
}

