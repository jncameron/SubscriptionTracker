using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionTracker.Application.DTOs
{
    public class CategoryBreakdownDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalCost { get; set; }
    }
}
