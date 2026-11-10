
using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class RegisterParticipantDto
    {
        [Required]
        public string ParticipantName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ParticipantEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}

