using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.Models
{
    public class EventFormViewModel
    {
        [Required]
        public string EventName { get; set; } = "";

        [Required]
        public DateOnly EventDate { get; set; } =
            DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public string Location { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Range(0.1, 1000)]
        public decimal? Distance { get; set; }

        [Required]
        public string EventType { get; set; } = "";
    }
}
