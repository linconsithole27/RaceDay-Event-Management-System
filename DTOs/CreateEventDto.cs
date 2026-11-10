using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class CreateEventDto
    {
        [Required]
        public string EventName { get; set; } = string.Empty;

        [Required]
        public DateOnly EventDate { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int OrganiserId { get; set; }

        public decimal? Distance { get; set; }

        public string? EventType { get; set; }
    }
}