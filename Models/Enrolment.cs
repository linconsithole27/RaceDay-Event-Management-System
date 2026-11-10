using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class Enrolment
{
    public int EnrolmentId { get; set; }

    public int ParticipantId { get; set; }

    public int CategoryId { get; set; }

    public DateOnly EnrolmentDate { get; set; }

    public virtual EventCategory Category { get; set; } = null!;

    public virtual Participat Participant { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
