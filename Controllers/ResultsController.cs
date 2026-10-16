
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.Models;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: api/Results
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Result>>> GetResults()
    {
        var results = await _context.Results.ToListAsync();
        return Ok(results);
    }

    // GET: api/Results/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Result>> GetResult(int id)
    {
        var result = await _context.Results
            .FirstOrDefaultAsync(r => r.Result_ID == id);

        if (result == null)
            return NotFound("Result not found.");

        return Ok(result);
    }

    // POST: api/Results
    [HttpPost]
    public async Task<ActionResult<Result>> CreateResult(
        [FromBody] Result result)
    {
        var enrolmentExists = await _context.Enrolments
            .AnyAsync(e => e.Enrolment_ID == result.Enrolment_ID);

        if (!enrolmentExists)
            return BadRequest("The enrolment does not exist.");

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetResult),
            new { id = result.Result_ID },
            result);
    }

    // PUT: api/Results/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateResult(
        int id,
        [FromBody] Result updatedResult)
    {
        if (id != updatedResult.Result_ID)
            return BadRequest("Result ID does not match.");

        var result = await _context.Results
            .FirstOrDefaultAsync(r => r.Result_ID == id);

        if (result == null)
            return NotFound("Result not found.");

        var enrolmentExists = await _context.Enrolments
            .AnyAsync(e => e.Enrolment_ID == updatedResult.Enrolment_ID);

        if (!enrolmentExists)
            return BadRequest("The enrolment does not exist.");

        result.Enrolment_ID = updatedResult.Enrolment_ID;
        result.Finish_Time = updatedResult.Finish_Time;
        result.Enrolment_Date = updatedResult.Enrolment_Date;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Results/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        var result = await _context.Results
            .FirstOrDefaultAsync(r => r.Result_ID == id);

        if (result == null)
            return NotFound("Result not found.");

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
