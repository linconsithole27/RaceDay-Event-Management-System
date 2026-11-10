
using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class RegisterOrganiserDto
    {
        [Required]
        public string OrganiserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string OrganiserEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}

