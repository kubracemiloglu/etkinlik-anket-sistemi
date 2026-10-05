using Microsoft.AspNetCore.Mvc;

namespace EtkinlikAnketSistemi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}