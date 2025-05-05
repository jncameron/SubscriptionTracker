using Microsoft.Extensions.Logging;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces.Repositories;
using SubscriptionTracker.Application.Interfaces.Services;
using SubscriptionTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionTracker.Application.Services
{
    public class SubscriptionService(ISubscriptionRepository subscriptionRepository, ICategoryService categoryService, ILogger<SubscriptionService> logger) : ISubscriptionService
    {
        public async Task<List<SubscriptionDTO>> GetAllSubscriptions()
        {
            logger.LogInformation("Retrieving all subscriptions from repository.");
            var subscriptions = await subscriptionRepository.GetAllSubscriptions();
            logger.LogInformation("Retrieved {Count} subscriptions.", subscriptions.Count());
            var subscriptionDTOs = new List<SubscriptionDTO>();
            foreach (var subscription in subscriptions)
            {
                var subscriptionDTO = new SubscriptionDTO
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    MonthlyCost = subscription.MonthlyCost,
                    StartDate = subscription.StartDate,
                    IsActive = subscription.IsActive,
                    CategoryName = subscription.Category.Name
                };
                subscriptionDTOs.Add(subscriptionDTO);
            }
            return subscriptionDTOs;
        }
        public async Task<Subscription?> CreateSubscription(SubscriptionDTO subscriptionDTO)
        {
            // Check if the category exists in the database  
            var existingCategory = await categoryService.GetCategoryByName(subscriptionDTO.CategoryName);
            
            Category category;

            if (existingCategory != null)
            {
                // Use the existing category  
                category = new Category                 {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name
                };
            }
            else
            {
                // Create a new category  
                category = new Category
                {
                    Name = subscriptionDTO.CategoryName
                };

                await categoryService.CreateCategory(category);
            }

            // Create the subscription  
            Subscription subscription = new Subscription
            {
                Name = subscriptionDTO.Name,
                MonthlyCost = subscriptionDTO.MonthlyCost,
                StartDate = subscriptionDTO.StartDate,
                IsActive = subscriptionDTO.IsActive,
                CategoryId = category.Id
            };

            var newSubscription = await subscriptionRepository.CreateSubscription(subscription);

            return newSubscription;
        }
        public async Task<SubscriptionDTO?> GetSubscriptionByIdAsync(int id)
        {
            var subscription = await subscriptionRepository.GetSubscriptionByIdAsync(id);
            if (subscription != null)
            {
                var subscriptionDTO = new SubscriptionDTO
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    MonthlyCost = subscription.MonthlyCost,
                    StartDate = subscription.StartDate,
                    IsActive = subscription.IsActive,
                    CategoryName = subscription.Category.Name
                };
                return subscriptionDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<SubscriptionDTO?> GetSubscriptionByNameAsync(string name)
        {
            var subscription = await subscriptionRepository.GetSubscriptionByNameAsync(name);
            if (subscription != null)
            {
                var subscriptionDTO = new SubscriptionDTO
                {
                    Id = subscription.Id,
                    Name = subscription.Name,
                    MonthlyCost = subscription.MonthlyCost,
                    StartDate = subscription.StartDate,
                    IsActive = subscription.IsActive,
                    CategoryName = subscription.Category.Name
                };
                return subscriptionDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateSubscriptionAsync(int id, SubscriptionDTO dto)
        {
            return await subscriptionRepository.UpdateSubscriptionAsync(id, dto);
        }


        public async Task<bool> DeleteSubscriptionAsync(int id)
        {
            var subscription = await subscriptionRepository.GetSubscriptionByIdAsync(id);
            if (subscription == null)
                return false;

            await subscriptionRepository.DeleteSubscriptionAsync(subscription);
            return true;
        }
    }
}
