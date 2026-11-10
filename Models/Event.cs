using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateOnly EventDate { get; set; }

    public string Locationn { get; set; } = null!;

    public string? Descriptionn { get; set; }

    public int OrganiserId { get; set; }

    public decimal? Distance { get; set; }

    public string? EventType { get; set; }

    public virtual ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();

    public virtual Organiser Organiser { get; set; } = null!;
}
