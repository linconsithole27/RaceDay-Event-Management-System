
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;
using RaceDay.Security;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/events
    [SessionRole("Organiser")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        var events = await _context.Events.ToListAsync();

        return Ok(events);
    }

    // GET: api/events/5
    [SessionRole("Organiser")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.Event_ID == id);

        if (eventItem == null)
        {
            return NotFound("Event not found.");
        }

        return Ok(eventItem);
    }

    // POST: api/events
    [SessionRole("Organiser")]
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(
        [FromBody] Event eventItem)
    {
        _context.Events.Add(eventItem);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEventById),
            new { id = eventItem.Event_ID },
            eventItem);
    }

    // PUT: api/events/5
    [SessionRole("Organiser")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        [FromBody] Event updatedEvent)
    {
        if (id != updatedEvent.Event_ID)
        {
            return BadRequest("Event ID does not match.");
        }

        var existingEvent = await _context.Events
            .FirstOrDefaultAsync(e => e.Event_ID == id);

        if (existingEvent == null)
        {
            return NotFound("Event not found.");
        }

        existingEvent.Event_Name = updatedEvent.Event_Name;
        existingEvent.Event_Date = updatedEvent.Event_Date;
        existingEvent.Location = updatedEvent.Location;
        existingEvent.Description = updatedEvent.Description;
        existingEvent.Organiser_ID = updatedEvent.Organiser_ID;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/events/5
    [SessionRole("Organiser")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.Event_ID == id);

        if (eventItem == null)
        {
            return NotFound("Event not found.");
        }

        _context.Events.Remove(eventItem);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
