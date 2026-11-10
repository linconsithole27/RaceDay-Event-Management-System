using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Data;
using RACEDAY.API.Models;
using RACEDAY.API.DTOs;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [AllowAnonymous]
        [Authorize(Roles = "Organiser,Participant")]
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // POST: api/events
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto eventDto)
        {
            bool organiserExists = await _context.Organisers
                .AnyAsync(o => o.OrganiserId == eventDto.OrganiserId);

            if (!organiserExists)
            {
                return NotFound("Organiser not found.");
            }

            int nextId = (await _context.Events
                .MaxAsync(e => (int?)e.EventId) ?? 0) + 1;

            var newEvent = new Event
            {
                EventId = nextId,
                EventName = eventDto.EventName,
                EventDate = eventDto.EventDate,
                Locationn = eventDto.Location,
                Descriptionn = eventDto.Description,
                OrganiserId = eventDto.OrganiserId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event created successfully",
                eventId = newEvent.EventId
            });
        }

        // PUT: api/events/{id}
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, CreateEventDto eventDto)
        {
            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            bool organiserExists = await _context.Organisers
                .AnyAsync(o => o.OrganiserId == eventDto.OrganiserId);

            if (!organiserExists)
            {
                return NotFound("Organiser not found.");
            }

            existingEvent.EventName = eventDto.EventName;
            existingEvent.EventDate = eventDto.EventDate;
            existingEvent.Locationn = eventDto.Location;
            existingEvent.Descriptionn = eventDto.Description;
            existingEvent.OrganiserId = eventDto.OrganiserId;

            await _context.SaveChangesAsync();

            return Ok("Event updated successfully.");
        }

        // DELETE: api/events/{id}
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            _context.Events.Remove(existingEvent);

            await _context.SaveChangesAsync();

            return Ok("Event deleted successfully.");
        }
    }
}

