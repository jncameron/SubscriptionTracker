using SubscriptionTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionTracker.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        public Task<List<Subscription>> GetAllSubscriptions();
        public Task<Subscription> CreateSubscription(Subscription subscription);
        public Task<Subscription> GetSubscriptionByIdAsync(int id);
        Task<Subscription> GetSubscriptionByNameAsync(string name);
    }
}
