using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class EventCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int EventId { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public virtual Event Event { get; set; } = null!;
}
