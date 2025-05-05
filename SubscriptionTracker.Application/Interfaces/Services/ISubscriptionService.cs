using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

namespace SubscriptionTracker.Application.Interfaces.Services
{
    public interface ISubscriptionService
    {
        Task<Subscription?> CreateSubscription(SubscriptionDTO subscriptionDTO);
        Task<List<SubscriptionDTO>> GetAllSubscriptions();
        Task<SubscriptionDTO?> GetSubscriptionByIdAsync(int id);
        Task<SubscriptionDTO?> GetSubscriptionByNameAsync(string name);
        Task<bool> UpdateSubscriptionAsync(int id, SubscriptionDTO dto);
        Task<bool> DeleteSubscriptionAsync(int id);

    }
}