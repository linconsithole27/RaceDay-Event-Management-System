using System;
using System.Collections.Generic;

namespace RACEDAY.API.Models;

public partial class Result
{
    public int ResultId { get; set; }

    public int EnrolmentId { get; set; }

    public TimeOnly FinishTime { get; set; }

    public DateOnly EnrolmentDate { get; set; }

    public int? Position { get; set; }

    public virtual Enrolment Enrolment { get; set; } = null!;
}
