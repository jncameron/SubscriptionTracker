using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces.Repositories;
using SubscriptionTracker.Domain.Entities;
using SubscriptionTracker.Infrastructure.Data;

namespace SubscriptionTracker.Infrastructure.Repositories
{
    public class SubscriptionRepository(SubscriptionAppContext appContext, IConfiguration configuration, ILogger<SubscriptionRepository> logger) : ISubscriptionRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        public async Task<List<Subscription>> GetAllSubscriptions(string userId)
        {
            return await appContext.Subscriptions
                .Include(s => s.Category)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
        public async Task<Subscription> CreateSubscription(Subscription subscription)
        {
            await appContext.Subscriptions.AddAsync(subscription);
            var result = await appContext.SaveChangesAsync();
            return subscription;
        }

        public async Task<Subscription?> GetSubscriptionByIdAsync(int id)
        {
            //return await appContext.Subscriptions.Include(s => s.Category).FirstOrDefaultAsync(x => x.Id == id);

            //Use ADO raw sql to fetch subscription by id

            const string sql = @"
            SELECT s.Id, s.Name, s.StartDate, s.MonthlyCost, s.IsActive, s.CategoryId,
                   c.Id AS CategoryId, c.Name AS CategoryName
            FROM Subscriptions s
            INNER JOIN Categories c ON s.CategoryId = c.Id
            WHERE s.Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            logger.LogInformation("Executing SQL query to fetch subscription by ID: {Id}\nQuery:\n{Query}", id, sql);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                logger.LogWarning("No subscription found with ID: {Id}", id);
                return null;

            }

            Subscription? subscription = null;

            while (await reader.ReadAsync())
            {
                subscription = new Subscription
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    MonthlyCost = reader.GetDecimal(reader.GetOrdinal("MonthlyCost")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    Category = new Category
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                    }
                };
            }

            return subscription;
        }
        

        public async Task<Subscription?> GetSubscriptionByNameAsync(string name,string userId)
        {
            return await appContext.Subscriptions
               .Include(s => s.Category).Where(x => x.UserId == userId)
               .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> UpdateSubscriptionAsync(int id, SubscriptionDTO dto)
        {
            var subscription = await appContext.Subscriptions.FindAsync(id);
            if (subscription == null)
                return false;

            // Update subscription properties
            subscription.Name = dto.Name;
            subscription.MonthlyCost = dto.MonthlyCost;
            subscription.IsActive = dto.IsActive;
            subscription.StartDate = dto.StartDate;

            // Handle Category update if necessary
            if (!string.IsNullOrEmpty(dto.CategoryName))
            {
                var category = await appContext.Categories
                    .FirstOrDefaultAsync(c => c.Name == dto.CategoryName);

                if (category == null)
                {
                    category = new Category { Name = dto.CategoryName };
                    appContext.Categories.Add(category);
                    await appContext.SaveChangesAsync();
                }

                subscription.CategoryId = category.Id;
            }

            appContext.Subscriptions.Update(subscription);
            await appContext.SaveChangesAsync();

            return true;
        }

        public async Task DeleteSubscriptionAsync(Subscription subscription)
        {
            appContext.Subscriptions.Remove(subscription);
            await appContext.SaveChangesAsync();
        }
    }
}
