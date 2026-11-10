
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Data;
using RACEDAY.API.Models;
using RACEDAY.API.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;
        private readonly PasswordHasher<Organiser> _organiserHasher;
        private readonly PasswordHasher<Participat> _participantHasher;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
            _organiserHasher = new PasswordHasher<Organiser>();
            _participantHasher = new PasswordHasher<Participat>();
        }

        // POST: api/auth/register/organiser
        [HttpPost("register/organiser")]
        public async Task<IActionResult> RegisterOrganiser(
            RegisterOrganiserDto dto)
        {
            bool emailExists = await _context.Organisers
                .AnyAsync(o => o.OrganiserEmail == dto.OrganiserEmail);

            if (emailExists)
                return BadRequest("Email already registered.");

            int nextId = (await _context.Organisers
                .MaxAsync(o => (int?)o.OrganiserId) ?? 0) + 1;

            var organiser = new Organiser
            {
                OrganiserId = nextId,
                OrganiserName = dto.OrganiserName,
                OrganiserEmail = dto.OrganiserEmail
            };

            organiser.Password = _organiserHasher.HashPassword(
                organiser, dto.Password);

            _context.Organisers.Add(organiser);
            await _context.SaveChangesAsync();

            return Ok("Organiser registered successfully.");
        }

        // POST: api/auth/register/participant
        [HttpPost("register/participant")]
        public async Task<IActionResult> RegisterParticipant(
            RegisterParticipantDto dto)
        {
            bool emailExists = await _context.Participats
                .AnyAsync(p => p.ParticipantEmail == dto.ParticipantEmail);

            if (emailExists)
                return BadRequest("Email already registered.");

            int nextId = (await _context.Participats
                .MaxAsync(p => (int?)p.ParticipantId) ?? 0) + 1;

            var participant = new Participat
            {
                ParticipantId = nextId,
                ParticipantName = dto.ParticipantName,
                ParticipantEmail = dto.ParticipantEmail
            };

            participant.Password = _participantHasher.HashPassword(
                participant, dto.Password);

            _context.Participats.Add(participant);
            await _context.SaveChangesAsync();

            return Ok("Participant registered successfully.");
        }


        // POST: api/auth/login/organiser
        [HttpPost("login/organiser")]
        public async Task<IActionResult> LoginOrganiser(LoginDto dto)
        {
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.OrganiserEmail == dto.Email);

            if (organiser == null)
                return Unauthorized("Invalid email or password.");

            var passwordResult = _organiserHasher.VerifyHashedPassword(
                organiser, organiser.Password, dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    organiser.OrganiserId.ToString()),
                new Claim(ClaimTypes.Name, organiser.OrganiserName),
                new Claim(ClaimTypes.Role, "Organiser")
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok("Organiser logged in successfully.");
        }

        // POST: api/auth/login/participant
        [HttpPost("login/participant")]
        public async Task<IActionResult> LoginParticipant(LoginDto dto)
        {
            var participant = await _context.Participats
                .FirstOrDefaultAsync(p => p.ParticipantEmail == dto.Email);

            if (participant == null)
                return Unauthorized("Invalid email or password.");

            var passwordResult = _participantHasher.VerifyHashedPassword(
                participant, participant.Password, dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    participant.ParticipantId.ToString()),
                new Claim(ClaimTypes.Name, participant.ParticipantName),
                new Claim(ClaimTypes.Role, "Participant")
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok("Participant logged in successfully.");
        }

        // GET: api/auth/me
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                name = User.Identity?.Name,
                role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return Ok("Logged out successfully.");
        }

    }
}