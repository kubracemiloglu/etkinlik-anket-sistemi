using EtkinlikAnketSistemi.Data;
using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int eventSurveyId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys
                .FirstOrDefaultAsync(x => x.Id == eventSurveyId);

            if (eventSurvey == null)
                return NotFound();

            ViewBag.EventSurveyId = eventSurveyId;
            ViewBag.EventTitle = eventSurvey.EventTitle;

            var questions = await _context.SurveyQuestions
                .Where(x => x.EventSurveyId == eventSurveyId)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();

            return View(questions);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int eventSurveyId)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys
                .FirstOrDefaultAsync(x => x.Id == eventSurveyId);

            if (eventSurvey == null)
                return NotFound();

            ViewBag.EventSurveyId = eventSurveyId;
            ViewBag.EventTitle = eventSurvey.EventTitle;

            var model = new SurveyQuestion
            {
                EventSurveyId = eventSurveyId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SurveyQuestion model)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var eventSurvey = await _context.EventSurveys
                    .FirstOrDefaultAsync(x => x.Id == model.EventSurveyId);

                ViewBag.EventSurveyId = model.EventSurveyId;
                ViewBag.EventTitle = eventSurvey?.EventTitle ?? "Etkinlik";

                return View(model);
            }

            _context.SurveyQuestions.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { eventSurveyId = model.EventSurveyId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var question = await _context.SurveyQuestions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
                return NotFound();

            var eventSurvey = await _context.EventSurveys
                .FirstOrDefaultAsync(x => x.Id == question.EventSurveyId);

            ViewBag.EventTitle = eventSurvey?.EventTitle ?? "Etkinlik";

            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SurveyQuestion model)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var eventSurvey = await _context.EventSurveys
                    .FirstOrDefaultAsync(x => x.Id == model.EventSurveyId);

                ViewBag.EventTitle = eventSurvey?.EventTitle ?? "Etkinlik";
                return View(model);
            }

            var question = await _context.SurveyQuestions
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (question == null)
                return NotFound();

            question.QuestionText = model.QuestionText;
            question.QuestionType = model.QuestionType;
            question.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { eventSurveyId = question.EventSurveyId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var question = await _context.SurveyQuestions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
                return NotFound();

            var eventSurvey = await _context.EventSurveys
                .FirstOrDefaultAsync(x => x.Id == question.EventSurveyId);

            ViewBag.EventTitle = eventSurvey?.EventTitle ?? "Etkinlik";

            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var question = await _context.SurveyQuestions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
                return NotFound();

            var eventSurveyId = question.EventSurveyId;

            _context.SurveyQuestions.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { eventSurveyId = eventSurveyId });
        }
    }
}