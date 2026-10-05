using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EtkinlikAnketSistemi.Models
{
    public class SurveyResponse
    {
        public int Id { get; set; }

        public int EventSurveyId { get; set; }

        [ValidateNever]
        public EventSurvey EventSurvey { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public List<SurveyAnswer> Answers { get; set; } = new();
    }
}