
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Data;
using RACEDAY.API.Models;
using RACEDAY.API.DTOs;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api/results")]
    [Authorize(Roles = "Organiser")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/results
        [HttpPost]
        public async Task<IActionResult> CreateResult(
            CreateResultDto resultDto)
        {
            bool enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentId == resultDto.EnrolmentId);

            if (!enrolmentExists)
                return NotFound("Enrolment not found.");

            bool resultExists = await _context.Results
                .AnyAsync(r => r.EnrolmentId == resultDto.EnrolmentId);

            if (resultExists)
                return BadRequest(
                    "A result already exists for this enrolment.");

            int nextId = (await _context.Results
                .MaxAsync(r => (int?)r.ResultId) ?? 0) + 1;

            var newResult = new Result
            {
                ResultId = nextId,
                EnrolmentId = resultDto.EnrolmentId,
                FinishTime = resultDto.FinishTime,
                Position = resultDto.Position,
                EnrolmentDate = DateOnly.FromDateTime(DateTime.Today)
            };

            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Result captured successfully",
                resultId = newResult.ResultId,
                position = newResult.Position
            });
        }
    }
}

