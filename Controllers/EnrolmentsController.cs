using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RACEDAY.API.Data;
using RACEDAY.API.Models;
using RACEDAY.API.DTOs;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api/enrolments")]
    [Authorize]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/enrolments
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(
            CreateEnrolmentDto enrolmentDto)
        {
            int participantId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            bool categoryExists = await _context.EventCategories
                .AnyAsync(c => c.CategoryId == enrolmentDto.CategoryId);

            if (!categoryExists)
                return NotFound("Category not found.");

            int nextId = (await _context.Enrolments
                .MaxAsync(e => (int?)e.EnrolmentId) ?? 0) + 1;

            var newEnrolment = new Enrolment
            {
                EnrolmentId = nextId,
                ParticipantId = participantId,
                CategoryId = enrolmentDto.CategoryId,
                EnrolmentDate = DateOnly.FromDateTime(DateTime.Today)
            };

            _context.Enrolments.Add(newEnrolment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Enrolment created successfully",
                enrolmentId = newEnrolment.EnrolmentId
            });
        }

        // GET: api/enrolments/participant/{participantId}
        [Authorize(Roles = "Participant")]
        [HttpGet("participant/{participantId}")]
        public async Task<IActionResult> GetParticipantEnrolments(
            int participantId)
        {
            int loggedInParticipantId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (participantId != loggedInParticipantId)
                return Forbid();

            var enrolments = await _context.Enrolments
                .Include(e => e.Category)
                .ThenInclude(c => c.Event)
                .Where(e => e.ParticipantId == loggedInParticipantId)
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/enrolments
        [Authorize(Roles = "Organiser")]
        [HttpGet]
        public async Task<IActionResult> GetAllEnrolments()
        {
            var enrolments = await _context.Enrolments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .ThenInclude(c => c.Event)
                .ToListAsync();

            return Ok(enrolments);
        }
    }
}