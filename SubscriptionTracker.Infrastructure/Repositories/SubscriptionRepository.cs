using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces;
using SubscriptionTracker.Domain.Entities;
using SubscriptionTracker.Infrastructure.Data;

namespace SubscriptionTracker.Infrastructure.Repositories
{
    public class SubscriptionRepository(SubscriptionAppContext appContext) : ISubscriptionRepository
    {
        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await appContext.Subscriptions.Include(s => s.Category).ToListAsync();
        }
        public async Task<Subscription> CreateSubscription(Subscription subscription)
        {
            await appContext.Subscriptions.AddAsync(subscription);
            var result = await appContext.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int id)
        {
            return await appContext.Subscriptions.Include(s => s.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Subscription> GetSubscriptionByNameAsync(string name)
        {
            return await appContext.Subscriptions
               .Include(s => s.Category)
               .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
