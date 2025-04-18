using CategoryTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Application.DTOs;
using SubscriptionTracker.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        // GET: api/<CategoryController>  
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        // POST api/<CategoryController>  
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] string value)
        {
            var category = new Category { Name = value };
            var result = await categoryService.CreateCategory(category);

            if (!result)
                return BadRequest("Failed to create category.");

            CategoryDTO responseObject = new CategoryDTO { Id = category.Id, Name = category.Name };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, responseObject);
        }
    }
}
