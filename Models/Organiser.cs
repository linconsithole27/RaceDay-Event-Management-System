using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class Organiser
{
    public int OrganiserId { get; set; }

    public string OrganiserName { get; set; } = null!;

    public string OrganiserEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
