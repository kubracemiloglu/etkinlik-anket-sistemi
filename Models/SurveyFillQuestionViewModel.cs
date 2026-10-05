namespace EtkinlikAnketSistemi.Models
{
    public class SurveyFillQuestionViewModel
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public string? Answer { get; set; }
    }
}