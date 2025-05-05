using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Application.Interfaces.Services;
using SubscriptionTracker.Application.Services;
using System.Security.Claims;

namespace SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dashboard(ISubscriptionService subscriptionService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var dashboard = await subscriptionService.GetDashboardAsync(userId);
            return Ok(dashboard);
        }
    }
}
