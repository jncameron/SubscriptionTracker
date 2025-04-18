using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

namespace SubscriptionTracker.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription> CreateSubscription(SubscriptionDTO subscriptionDTO);
        Task<List<SubscriptionDTO>> GetAllSubscriptions();
        Task<SubscriptionDTO> GetSubscriptionByIdAsync(int id);
        Task<SubscriptionDTO> GetSubscriptionByNameAsync(string name);
    }
}