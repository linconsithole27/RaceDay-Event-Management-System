
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;
using RaceDay.Security;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/Enrolments
    [SessionRole("Organiser")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
    {
        var enrolments = await _context.Enrolments.ToListAsync();
        return Ok(enrolments);
    }

    // GET: api/Enrolments/1
    [SessionRole("Organiser")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.Enrolment_ID == id);

        if (enrolment == null)
            return NotFound("Enrolment not found.");

        return Ok(enrolment);
    }

    // POST: api/Enrolments
    [SessionRole("Participant")]
    [HttpPost]
    public async Task<ActionResult<Enrolment>> CreateEnrolment(
        [FromBody] Enrolment enrolment)
    {
        enrolment.Enrolment_Date = DateTime.Today;

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEnrolment),
            new { id = enrolment.Enrolment_ID },
            enrolment);
    }

    // PUT: api/Enrolments/1
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEnrolment(
        int id,
        [FromBody] Enrolment updatedEnrolment)
    {
        if (id != updatedEnrolment.Enrolment_ID)
            return BadRequest("Enrolment ID does not match.");

        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.Enrolment_ID == id);

        if (enrolment == null)
            return NotFound("Enrolment not found.");

        enrolment.Participant_ID = updatedEnrolment.Participant_ID;
        enrolment.Category_ID = updatedEnrolment.Category_ID;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Enrolments/1
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEnrolment(int id)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(e => e.Enrolment_ID == id);

        if (enrolment == null)
            return NotFound("Enrolment not found.");

        _context.Enrolments.Remove(enrolment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
