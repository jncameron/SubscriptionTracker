namespace SubscriptionTracker.Tests;

using global::SubscriptionTracker.API.Controllers;
using global::SubscriptionTracker.Application.DTOs;
using global::SubscriptionTracker.Application.Interfaces.Services;
using global::SubscriptionTracker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockCategoryService;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockCategoryService = new Mock<ICategoryService>();
        _controller = new CategoryController(_mockCategoryService.Object);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsOkResult_WithListOfCategories()
    {
        // Arrange
        var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "Category1" },
                new CategoryDTO { Id = 2, Name = "Category2" }
            };
        _mockCategoryService.Setup(service => service.GetAllCategories())
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetAllCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategories = Assert.IsType<List<CategoryDTO>>(okResult.Value);
        Assert.Equal(2, returnedCategories.Count);
    }

    [Fact]
    public async Task GetCategory_ReturnsOkResult_WithCategory()
    {
        // Arrange
        var category = new CategoryDTO { Id = 1, Name = "Category1" };
        _mockCategoryService.Setup(service => service.GetCategoryById(1))
            .ReturnsAsync(category);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategory = Assert.IsType<CategoryDTO>(okResult.Value);
        Assert.Equal(1, returnedCategory.Id);
        Assert.Equal("Category1", returnedCategory.Name);
    }

    [Fact]
    public async Task GetCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.GetCategoryById(1))
            .ReturnsAsync((CategoryDTO?)null);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedAtActionResult_WithCreatedCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "NewCategory" };
        _mockCategoryService.Setup(service => service.CreateCategory(It.IsAny<Category>()))
            .Returns<Category>(cat =>
            {
                cat.Id = 1;
                return Task.FromResult(true);
            });

        // Act
        var result = await _controller.CreateCategory("NewCategory");

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedCategory = Assert.IsType<CategoryDTO>(createdResult.Value);
        Assert.Equal(1, returnedCategory.Id);
        Assert.Equal("NewCategory", returnedCategory.Name);
    }

    [Fact]
    public async Task CreateCategory_ReturnsBadRequest_WhenCreationFails()
    {
        // Arrange
        _mockCategoryService.Setup(service => service.CreateCategory(It.IsAny<Category>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateCategory("NewCategory");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to create category.", badRequestResult.Value);
    }
}


