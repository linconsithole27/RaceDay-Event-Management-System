
using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class CreateResultDto
    {
        [Required]
        public int EnrolmentId { get; set; }

        [Required]
        public TimeOnly FinishTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Position { get; set; }
    }
}
