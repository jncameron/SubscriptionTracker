using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

namespace SubscriptionTracker.Application.Interfaces.Services
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDTO?> CreateSubscription(SubscriptionDTO subscriptionDTO, string userId);
        Task<List<SubscriptionDTO>> GetAllSubscriptions(string userId);
        Task<SubscriptionDTO?> GetSubscriptionByIdAsync(int id);
        Task<SubscriptionDTO?> GetSubscriptionByNameAsync(string name, string userId);
        Task<bool> UpdateSubscriptionAsync(int id, SubscriptionDTO dto);
        Task<bool> DeleteSubscriptionAsync(int id);
        Task<IEnumerable<SubscriptionDTO>> GetByUserIdAsync(string userId);
        Task<DashboardDTO> GetDashboardAsync(string userId);
    }
}