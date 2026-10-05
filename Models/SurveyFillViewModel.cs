using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EtkinlikAnketSistemi.Models
{
    public class SurveyFillViewModel
    {
        public int EventSurveyId { get; set; }

        [ValidateNever]
        public string ProgramName { get; set; } = null!;

        [ValidateNever]
        public string EventTitle { get; set; } = null!;

        [ValidateNever]
        public DateTime EventDate { get; set; }

        public List<SurveyFillQuestionViewModel> Questions { get; set; } = new();
    }
}