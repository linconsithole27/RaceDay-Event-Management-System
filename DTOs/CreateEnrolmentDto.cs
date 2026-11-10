
using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class CreateEnrolmentDto
    {
        [Required]
        public int ParticipantId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
