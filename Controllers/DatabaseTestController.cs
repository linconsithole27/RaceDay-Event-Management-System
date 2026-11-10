using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RACEDAY.API.Data;

namespace RACEDAY.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public DatabaseTestController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            bool connected = await _context.Database.CanConnectAsync();

            if (connected)
            {
                return Ok("Successfully connected to RACEDAYEVENT_DB!");
            }

            return StatusCode(500, "Could not connect to RACEDAYEVENT_DB.");
        }
    }
}