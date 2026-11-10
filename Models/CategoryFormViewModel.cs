using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.Models
{
    public class CategoryFormViewModel
    {
        [Required]
        public string CategoryName { get; set; } = "";
    }
}
