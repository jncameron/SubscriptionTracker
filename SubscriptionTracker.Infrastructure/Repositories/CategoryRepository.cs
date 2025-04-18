using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Domain.Entities;
using SubscriptionTracker.Infrastructure.Data;

namespace SubscriptionTracker.Infrastructure.Repositories
{
    public class CategoryRepository(SubscriptionAppContext appContext) : ICategoryRepository
    {
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await appContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await appContext.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await appContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await appContext.Categories.AddAsync(category);
            var result = await appContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
