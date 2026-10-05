using EtkinlikAnketSistemi.Data;
using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int eventSurveyId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == eventSurveyId);

            if (eventSurvey == null)
                return NotFound();

            var responses = await _context.SurveyResponses
                .Where(x => x.EventSurveyId == eventSurveyId)
                .Include(x => x.Answers)
                .ToListAsync();

            var likertOrder = new List<string>
            {
                "Kesinlikle Katılıyorum",
                "Katılıyorum",
                "Kararsızım",
                "Katılmıyorum",
                "Kesinlikle Katılmıyorum"
            };

            var model = new ReportPageViewModel
            {
                EventSurveyId = eventSurvey.Id,
                ProgramName = eventSurvey.ProgramName,
                EventTitle = eventSurvey.EventTitle,
                EventDate = eventSurvey.EventDate,
                TotalParticipants = responses.Count,
                Questions = new List<ReportQuestionViewModel>()
            };

            foreach (var question in eventSurvey.Questions.OrderBy(q => q.DisplayOrder))
            {
                var questionModel = new ReportQuestionViewModel
                {
                    QuestionId = question.Id,
                    DisplayOrder = question.DisplayOrder,
                    QuestionText = question.QuestionText,
                    QuestionType = question.QuestionType
                };

                if (question.QuestionType == "Likert")
                {
                    var answersForQuestion = responses
                        .SelectMany(r => r.Answers)
                        .Where(a => a.SurveyQuestionId == question.Id && !string.IsNullOrWhiteSpace(a.SelectedOption))
                        .ToList();

                    foreach (var option in likertOrder)
                    {
                        var count = answersForQuestion.Count(a => a.SelectedOption == option);

                        questionModel.LikertResults.Add(new ReportOptionResultViewModel
                        {
                            OptionText = option,
                            Count = count
                        });
                    }
                }
                else if (question.QuestionType == "Text")
                {
                    questionModel.TextAnswers = responses
                        .SelectMany(r => r.Answers)
                        .Where(a => a.SurveyQuestionId == question.Id && !string.IsNullOrWhiteSpace(a.TextAnswer))
                        .Select(a => a.TextAnswer!.Trim())
                        .ToList();
                }

                model.Questions.Add(questionModel);
            }

            return View(model);
        }
    }
}