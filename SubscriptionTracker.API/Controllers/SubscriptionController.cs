using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces.Services;

namespace SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var subscriptions = await subscriptionService.GetAllSubscriptions();
            return Ok(subscriptions);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var subscription = await subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
                return NotFound();
            return Ok(subscription);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetSubscription(string subName)
        {
            var subscription = await subscriptionService.GetSubscriptionByNameAsync(subName);
            if (subscription == null)
                return NotFound();
            return Ok(subscription);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {
            var existingSubscription = (await subscriptionService.GetSubscriptionByNameAsync(subscriptionDTO.Name));

            if (existingSubscription != null)
            {
                return BadRequest(new { Message = "A subscription with the same name already exists." });
            }

            var created = await subscriptionService.CreateSubscription(subscriptionDTO);

            if (created == null)
            {
                return StatusCode(500, new { Message = "Failed to create subscription." });
            }

            return CreatedAtAction(nameof(GetSubscription), new { id = created.Id }, new SubscriptionDTO
            {
                Id = created.Id,
                Name = created.Name,
                MonthlyCost = created.MonthlyCost,
                StartDate = created.StartDate,
                IsActive = created.IsActive,
                CategoryName = created.Category?.Name ?? subscriptionDTO.CategoryName
            });
        }
    }
}
