using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Domain.Entities;


namespace SubscriptionTracker.Infrastructure.Data
{
    public class SubscriptionAppContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public SubscriptionAppContext(DbContextOptions<SubscriptionAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Subscription>().ToTable("Subscriptions");
            base.OnModelCreating(modelBuilder);
        }
    }
}
