
using System.ComponentModel.DataAnnotations;

namespace RACEDAY.API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}
