using EtkinlikAnketSistemi.Data;
using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Fill(int id)
        {
            var eventSurvey = await _context.EventSurveys
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (eventSurvey == null)
                return NotFound();

            var model = new SurveyFillViewModel
            {
                EventSurveyId = eventSurvey.Id,
                ProgramName = eventSurvey.ProgramName,
                EventTitle = eventSurvey.EventTitle,
                EventDate = eventSurvey.EventDate,
                Questions = eventSurvey.Questions
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new SurveyFillQuestionViewModel
                    {
                        QuestionId = x.Id,
                        QuestionText = x.QuestionText,
                        QuestionType = x.QuestionType,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Fill(SurveyFillViewModel model)
        {
            var eventSurvey = await _context.EventSurveys
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(x => x.Id == model.EventSurveyId && x.IsActive);

            if (eventSurvey == null)
                return NotFound();

            if (model.Questions == null || !model.Questions.Any())
            {
                ModelState.AddModelError("", "Anket soruları bulunamadı.");
                return View(model);
            }

            foreach (var question in model.Questions)
            {
                if (string.IsNullOrWhiteSpace(question.Answer) || question.Answer.Length > 4000)
                {
                    ModelState.AddModelError("", "Lütfen tüm soruları cevaplayınız.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                model.ProgramName = eventSurvey.ProgramName;
                model.EventTitle = eventSurvey.EventTitle;
                model.EventDate = eventSurvey.EventDate;

                var dbQuestions = eventSurvey.Questions.OrderBy(x => x.DisplayOrder).ToList();

                foreach (var q in model.Questions)
                {
                    var dbQuestion = dbQuestions.FirstOrDefault(x => x.Id == q.QuestionId);
                    if (dbQuestion != null)
                    {
                        q.QuestionText = dbQuestion.QuestionText;
                        q.QuestionType = dbQuestion.QuestionType;
                        q.DisplayOrder = dbQuestion.DisplayOrder;
                    }
                }

                return View(model);
            }

            var response = new SurveyResponse
            {
                EventSurveyId = model.EventSurveyId,
                SubmittedAt = DateTime.Now
            };

            _context.SurveyResponses.Add(response);
            await _context.SaveChangesAsync();

            foreach (var item in model.Questions)
            {
                var dbQuestion = eventSurvey.Questions.FirstOrDefault(x => x.Id == item.QuestionId);

                if (dbQuestion == null)
                    continue;

                var answer = new SurveyAnswer
                {
                    SurveyResponseId = response.Id,
                    SurveyQuestionId = item.QuestionId
                };

                if (dbQuestion.QuestionType == "Likert")
                {
                    answer.SelectedOption = item.Answer;
                }
                else if (dbQuestion.QuestionType == "Text")
                {
                    answer.TextAnswer = item.Answer;
                }

                _context.SurveyAnswers.Add(answer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Thanks", new { id = model.EventSurveyId });
        }

        public async Task<IActionResult> Thanks(int id)
        {
            var eventSurvey = await _context.EventSurveys.FirstOrDefaultAsync(x => x.Id == id);

            if (eventSurvey == null)
                return NotFound();

            var model = new SurveyThanksViewModel
            {
                EventTitle = eventSurvey.EventTitle
            };

            return View(model);
        }
    }
}