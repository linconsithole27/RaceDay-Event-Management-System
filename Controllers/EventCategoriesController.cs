
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;
using RaceDay.Security;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventCategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventCategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/EventCategories
    [SessionRole("Organiser")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventCategory>>> GetCategories()
    {
        var categories = await _context.EventCategories.ToListAsync();
        return Ok(categories);
    }

    // GET: api/EventCategories/1
    [SessionRole("Organiser")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventCategory>> GetCategory(int id)
    {
        var category = await _context.EventCategories
            .FirstOrDefaultAsync(c => c.Category_ID == id);

        if (category == null)
            return NotFound("Category not found.");

        return Ok(category);
    }

    // POST: api/EventCategories
    [SessionRole("Organiser")]
    [HttpPost]
    public async Task<ActionResult<EventCategory>> CreateCategory(
        [FromBody] EventCategory category)
    {
        _context.EventCategories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.Category_ID },
            category);
    }

    // PUT: api/EventCategories/1
    [SessionRole("Organiser")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        [FromBody] EventCategory updatedCategory)
    {
        if (id != updatedCategory.Category_ID)
            return BadRequest("Category ID does not match.");

        var category = await _context.EventCategories
            .FirstOrDefaultAsync(c => c.Category_ID == id);

        if (category == null)
            return NotFound("Category not found.");

        category.Category_Name = updatedCategory.Category_Name;
        category.Event_ID = updatedCategory.Event_ID;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/EventCategories/1
    [SessionRole("Organiser")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.EventCategories
            .FirstOrDefaultAsync(c => c.Category_ID == id);

        if (category == null)
            return NotFound("Category not found.");

        _context.EventCategories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
