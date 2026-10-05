using Microsoft.AspNetCore.Mvc;

namespace EtkinlikAnketSistemi.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");

            if (adminId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}