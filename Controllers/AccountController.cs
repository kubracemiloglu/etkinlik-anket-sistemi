using EtkinlikAnketSistemi.Data;
using Microsoft.AspNetCore.Identity;
using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikAnketSistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = await _context.AdminUsers
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (admin == null || new PasswordHasher<AdminUser>().VerifyHashedPassword(admin, admin.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "E-posta veya şifre hatalı.";
                return View(model);
            }

            HttpContext.Session.SetInt32("AdminId", admin.Id);
            HttpContext.Session.SetString("AdminName", admin.FullName);
            HttpContext.Session.SetString("AdminEmail", admin.Email);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}