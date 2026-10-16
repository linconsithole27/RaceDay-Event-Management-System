
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ParticipantsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
    {
        return await _context.Participants
            .Select(p => new Participant
            {
                Participant_ID = p.Participant_ID,
                Participant_Name = p.Participant_Name,
                Participant_Email = p.Participant_Email,
                Password = ""
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Participant>> GetParticipant(int id)
    {
        var participant = await _context.Participants
            .Where(p => p.Participant_ID == id)
            .Select(p => new Participant
            {
                Participant_ID = p.Participant_ID,
                Participant_Name = p.Participant_Name,
                Participant_Email = p.Participant_Email,
                Password = ""
            })
            .FirstOrDefaultAsync();

        if (participant == null)
            return NotFound();

        return participant;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParticipant(int id, Participant participant)
    {
        if (id != participant.Participant_ID)
            return BadRequest("ID does not match.");

        var existing = await _context.Participants.FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Participant_Name = participant.Participant_Name;
        existing.Participant_Email = participant.Participant_Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParticipant(int id)
    {
        var participant = await _context.Participants.FindAsync(id);

        if (participant == null)
            return NotFound();

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
