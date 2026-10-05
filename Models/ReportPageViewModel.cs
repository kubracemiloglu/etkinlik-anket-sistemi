namespace EtkinlikAnketSistemi.Models
{
    public class ReportPageViewModel
    {
        public int EventSurveyId { get; set; }
        public string ProgramName { get; set; } = null!;
        public string EventTitle { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public int TotalParticipants { get; set; }

        public List<ReportQuestionViewModel> Questions { get; set; } = new();
    }
}