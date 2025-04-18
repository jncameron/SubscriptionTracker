using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

namespace CategoryTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> CreateCategory(Category category);
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> GetCategoryByName(string name);
        Task<List<CategoryDTO>> GetAllCategories();
    }
}