using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class Participat
{
    public int ParticipantId { get; set; }

    public string ParticipantName { get; set; } = null!;

    public string ParticipantEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
}
