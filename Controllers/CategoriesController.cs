using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Data;
using RACEDAY.API.Models;
using RACEDAY.API.DTOs;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events/{eventId}/categories
        [Authorize(Roles = "Organiser,Participant")]
        [HttpGet("events/{eventId}/categories")]
        public async Task<IActionResult> GetCategories(int eventId)
        {
            // Check whether the event exists
            bool eventExists = await _context.Events
                .AnyAsync(e => e.EventId == eventId);

            if (!eventExists)
            {
                return NotFound("Event not found.");
            }

            // Retrieve categories belonging to the event
            var categories = await _context.EventCategories
                .Where(c => c.EventId == eventId)
                .ToListAsync();

            return Ok(categories);
        }

        // POST: api/events/{eventId}/categories
        [Authorize(Roles = "Organiser")]
        [HttpPost("events/{eventId}/categories")]
        public async Task<IActionResult> CreateCategory(
            int eventId, CreateCategoryDto categoryDto)
        {
            // Check whether the event exists
            bool eventExists = await _context.Events
                .AnyAsync(e => e.EventId == eventId);

            if (!eventExists)
            {
                return NotFound("Event not found.");
            }

            // Generate the next Category ID
            int nextId = (await _context.EventCategories
                .MaxAsync(c => (int?)c.CategoryId) ?? 0) + 1;

            // Create the category
            var newCategory = new EventCategory
            {
                CategoryId = nextId,
                CategoryName = categoryDto.CategoryName,
                EventId = eventId
            };

            // Save category
            _context.EventCategories.Add(newCategory);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category created successfully",
                categoryId = newCategory.CategoryId
            });
        }

        // PUT: api/categories/{id}
        [Authorize(Roles = "Organiser")]
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id, CreateCategoryDto categoryDto)
        {
            var existingCategory = await _context.EventCategories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            existingCategory.CategoryName = categoryDto.CategoryName;

            await _context.SaveChangesAsync();

            return Ok("Category updated successfully.");
        }

        // DELETE: api/categories/{id}
        [Authorize(Roles = "Organiser")]
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _context.EventCategories
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }

            _context.EventCategories.Remove(existingCategory);

            await _context.SaveChangesAsync();

            return Ok("Category deleted successfully.");
        }
    }
}
