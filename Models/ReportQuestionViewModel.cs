namespace EtkinlikAnketSistemi.Models
{
    public class ReportQuestionViewModel
    {
        public int QuestionId { get; set; }
        public int DisplayOrder { get; set; }
        public string QuestionText { get; set; } = null!;
        public string QuestionType { get; set; } = null!;

        public List<ReportOptionResultViewModel> LikertResults { get; set; } = new();
        public List<string> TextAnswers { get; set; } = new();
    }
}