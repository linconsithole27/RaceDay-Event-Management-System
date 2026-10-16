
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RaceDay.Data;
using RaceDay.DTOs;
using RaceDay.Models;

namespace RaceDay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    private readonly PasswordHasher<string> _hasher = new();

    public AuthController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.AccountType))
        {
            return BadRequest("All fields are required.");
        }

        string type = request.AccountType.Trim().ToLower();
        string email = request.Email.Trim();

        if (type == "organiser")
        {
            if (await _context.Organisers.AnyAsync(x => x.Organiser_Email == email))
                return Conflict("Email already registered.");

            int id = (await _context.Organisers
                .MaxAsync(x => (int?)x.Organiser_ID) ?? 0) + 1;

            var organiser = new Organiser
            {
                Organiser_ID = id,
                Organiser_Name = request.Name.Trim(),
                Organiser_Email = email,
                Password = _hasher.HashPassword(email, request.Password)
            };

            _context.Organisers.Add(organiser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Organiser registered successfully." });
        }

        if (type == "participant")
        {
            if (await _context.Participants.AnyAsync(x => x.Participant_Email == email))
                return Conflict("Email already registered.");

            int id = (await _context.Participants
                .MaxAsync(x => (int?)x.Participant_ID) ?? 0) + 1;

            var participant = new Participant
            {
                Participant_ID = id,
                Participant_Name = request.Name.Trim(),
                Participant_Email = email,
                Password = _hasher.HashPassword(email, request.Password)
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Participant registered successfully." });
        }

        return BadRequest("AccountType must be Organiser or Participant.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.AccountType))
        {
            return BadRequest("Email, password and account type are required.");
        }

        string email = request.Email.Trim();
        string type = request.AccountType.Trim().ToLower();

        if (type == "organiser")
        {
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(x => x.Organiser_Email == email);

            if (organiser == null)
                return Unauthorized("Invalid email or password.");

            var result = _hasher.VerifyHashedPassword(
                email, organiser.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            return Ok(new
            {
                message = "Login successful.",
                accountType = "Organiser",
                id = organiser.Organiser_ID,
                name = organiser.Organiser_Name
            });
        }

        if (type == "participant")
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(x => x.Participant_Email == email);

            if (participant == null)
                return Unauthorized("Invalid email or password.");

            var result = _hasher.VerifyHashedPassword(
                email, participant.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            return Ok(new
            {
                message = "Login successful.",
                accountType = "Participant",
                id = participant.Participant_ID,
                name = participant.Participant_Name


[HttpPost("login")]
public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.AccountType))
        {
            return BadRequest("Email, password and account type are required.");
        }

        string email = request.Email.Trim();
        string type = request.AccountType.Trim().ToLower();

        string? userId = null;
        string? userName = null;
        string? role = null;
        string? storedPassword = null;

        if (type == "organiser")
        {
            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(x => x.Organiser_Email == email);

            if (organiser != null)
            {
                userId = organiser.Organiser_ID.ToString();
                userName = organiser.Organiser_Name;
                role = "Organiser";
                storedPassword = organiser.Password;
            }
        }
        else if (type == "participant")
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(x => x.Participant_Email == email);

            if (participant != null)
            {
                userId = participant.Participant_ID.ToString();
                userName = participant.Participant_Name;
                role = "Participant";
                storedPassword = participant.Password;
            }
        }
        else
        {
            return BadRequest("AccountType must be Organiser or Participant.");
        }

        if (userId == null || storedPassword == null)
            return Unauthorized("Invalid email or password.");

        var passwordResult = _hasher.VerifyHashedPassword(
            email, storedPassword, request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid email or password.");

        // Remember the authenticated user for later requests
        HttpContext.Session.SetString("UserId", userId);
        HttpContext.Session.SetString("UserName", userName!);
        HttpContext.Session.SetString("UserRole", role!);

        return Ok(new
        {
            message = "Login successful.",
            id = userId,
            name = userName,
            role = role
        });
    }

});
        }

        return BadRequest("AccountType must be Organiser or Participant.");
    }
}
