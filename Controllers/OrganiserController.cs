
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganisersController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public OrganisersController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrganisers()
    {
        var organisers = await _context.Organisers
            .Select(o => new
            {
                o.Organiser_ID,
                o.Organiser_Name,
                o.Organiser_Email
            })
            .ToListAsync();

        return Ok(organisers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganiser(int id)
    {
        var organiser = await _context.Organisers
            .Where(o => o.Organiser_ID == id)
            .Select(o => new
            {
                o.Organiser_ID,
                o.Organiser_Name,
                o.Organiser_Email
            })
            .FirstOrDefaultAsync();

        if (organiser == null)
            return NotFound();

        return Ok(organiser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrganiser(
        int id, Organiser organiser)
    {
        if (id != organiser.Organiser_ID)
            return BadRequest("ID does not match.");

        var existing = await _context.Organisers.FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Organiser_Name = organiser.Organiser_Name;
        existing.Organiser_Email = organiser.Organiser_Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrganiser(int id)
    {
        var organiser = await _context.Organisers.FindAsync(id);

        if (organiser == null)
            return NotFound();

        _context.Organisers.Remove(organiser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
