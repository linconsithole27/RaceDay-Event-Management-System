
namespace RaceDay.Mvc.Models
{
    public class OrganiserEnrolmentViewModel
    {
        public int EnrolmentId { get; set; }
        public int ParticipantId { get; set; }
        public int CategoryId { get; set; }
        public DateOnly EnrolmentDate { get; set; }

        public EnrolmentParticipantViewModel? Participant { get; set; }
        public EnrolmentCategoryViewModel? Category { get; set; }
    }

    public class EnrolmentParticipantViewModel
    {
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = "";
        public string ParticipantEmail { get; set; } = "";
    }

    public class EnrolmentCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public EnrolmentEventViewModel? Event { get; set; }
    }

    public class EnrolmentEventViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = "";
    }
}