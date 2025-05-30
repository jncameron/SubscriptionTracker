﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Application.Interfaces.Services;
using SubscriptionTracker.Application.Services;
using System.Security.Claims;

namespace SubscriptionTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(ISubscriptionService subscriptionService, ILogger<SubscriptionController> logger) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var subscriptions = await subscriptionService.GetAllSubscriptions(userId);
            logger.LogInformation("Fetched {Count} subscriptions", subscriptions.Count);
            return Ok(subscriptions);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var subscription = await subscriptionService.GetSubscriptionByIdAsync(id);
            if (subscription == null)
                return NotFound();
            return Ok(subscription);
        }

        [Authorize]
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetSubscription(string subName)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var subscription = await subscriptionService.GetSubscriptionByNameAsync(subName, userId);
            if (subscription == null)
                return NotFound();
            return Ok(subscription);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            var existingSubscription = (await subscriptionService.GetSubscriptionByNameAsync(subscriptionDTO.Name, userId));

            if (existingSubscription != null)
            {
                return BadRequest(new { Message = "A subscription with the same name already exists." });
            }

            var created = await subscriptionService.CreateSubscription(subscriptionDTO, userId);

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
                CategoryName = created.CategoryName ?? subscriptionDTO.CategoryName
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, [FromBody] SubscriptionDTO dto)
        {
            var success = await subscriptionService.UpdateSubscriptionAsync(id, dto);
            if (!success)
                return NotFound();

            logger.LogInformation("Updated subscription with ID {Id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var success = await subscriptionService.DeleteSubscriptionAsync(id);
            if (!success)
            {
                return NotFound();
            }

            logger.LogInformation("Deleted subscription with ID {Id}", id);
            return NoContent();
        }

    }
}
