using SubscriptionTracker.Domain.Entities;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task<Category> GetCategoryByNameAsync(string name);
    Task<bool> CreateCategory(Category category);
}