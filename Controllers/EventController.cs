using EtkinlikAnketSistemi.Data;
using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var events = await _context.EventSurveys
                .OrderByDescending(x => x.EventDate)
                .ToListAsync();

            return View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventSurvey model)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            _context.EventSurveys.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys.FindAsync(id);

            if (eventSurvey == null)
                return NotFound();

            return View(eventSurvey);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventSurvey model)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var eventSurvey = await _context.EventSurveys.FindAsync(model.Id);

            if (eventSurvey == null)
                return NotFound();

            eventSurvey.ProgramName = model.ProgramName;
            eventSurvey.EventTitle = model.EventTitle;
            eventSurvey.EventDate = model.EventDate;
            eventSurvey.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys
                .FirstOrDefaultAsync(x => x.Id == id);

            if (eventSurvey == null)
                return NotFound();

            return View(eventSurvey);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
                return RedirectToAction("Login", "Account");

            var eventSurvey = await _context.EventSurveys.FindAsync(id);

            if (eventSurvey == null)
                return NotFound();

            _context.EventSurveys.Remove(eventSurvey);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}