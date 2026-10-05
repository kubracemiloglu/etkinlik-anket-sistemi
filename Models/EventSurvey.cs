using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EtkinlikAnketSistemi.Models
{
    public class EventSurvey
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Program adı zorunludur.")]
        public string ProgramName { get; set; } = null!;

        [Required(ErrorMessage = "Etkinlik adı zorunludur.")]
        public string EventTitle { get; set; } = null!;

        public DateTime EventDate { get; set; }

        public bool IsActive { get; set; } = true;

        [ValidateNever]
        public List<SurveyQuestion> Questions { get; set; } = new();
    }
}