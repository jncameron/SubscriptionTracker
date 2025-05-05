using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionTracker.Application.DTOs
{
    public class DashboardDTO
    {
        public decimal TotalMonthlyCost { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }

        public List<CategoryBreakdownDTO> Categories { get; set; } = new();
    }
}
