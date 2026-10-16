
namespace RaceDay.Models;

public class Result
{
    public int Result_ID { get; set; }
    public int Enrolment_ID { get; set; }
    public TimeSpan Finish_Time { get; set; }
    public DateTime Enrolment_Date { get; set; }
}
