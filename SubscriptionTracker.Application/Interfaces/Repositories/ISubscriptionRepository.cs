using SubscriptionTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionTracker.Application.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<List<Subscription>> GetAllSubscriptions();
        Task<Subscription> CreateSubscription(Subscription subscription);
        Task<Subscription?> GetSubscriptionByIdAsync(int id);
        Task<Subscription?> GetSubscriptionByNameAsync(string name);
    }
}
