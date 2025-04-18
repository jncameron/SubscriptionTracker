using CategoryTracker.Application.Interfaces;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

namespace SubscriptionTracker.Application.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllCategoriesAsync();
            var categoryDTOs = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                categoryDTOs.Add(categoryDTO);
            }
            return categoryDTOs;
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);
            if (category != null)
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                return categoryDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            var category = await categoryRepository.GetCategoryByNameAsync(name);
            if (category != null)
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                return categoryDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CreateCategory(Category category)
        {
            var newCategory = await categoryRepository.CreateCategory(category);
            return newCategory;
        }
    }
}
