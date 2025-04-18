
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionTracker.Domain.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyCost { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }

        public  int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
