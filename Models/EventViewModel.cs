namespace RaceDay.Mvc.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }

        public string EventName { get; set; } = "";

        public DateOnly EventDate { get; set; }

        public string Locationn { get; set; } = "";

        public string? Descriptionn { get; set; }

        public int OrganiserId { get; set; }

        public decimal? Distance { get; set; }

        public string? EventType { get; set; }
    }
}
