using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EtkinlikAnketSistemi.Models
{
    public class SurveyAnswer
    {
        public int Id { get; set; }

        public int SurveyResponseId { get; set; }

        [ValidateNever]
        public SurveyResponse SurveyResponse { get; set; } = null!;

        public int SurveyQuestionId { get; set; }

        [ValidateNever]
        public SurveyQuestion SurveyQuestion { get; set; } = null!;

        public string? SelectedOption { get; set; }

        public string? TextAnswer { get; set; }
    }
}