using EtkinlikAnketSistemi.Models;
using Microsoft.AspNetCore.Identity;
namespace EtkinlikAnketSistemi.Data;
public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        // Only an explicitly configured database is initialized; use a separate demo database.
        context.Database.EnsureCreated();
        if (context.AdminUsers.Any()) return;
        var email = Environment.GetEnvironmentVariable("DEMO_ADMIN_EMAIL");
        var password = Environment.GetEnvironmentVariable("DEMO_ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length < 12)
            throw new InvalidOperationException("Set DEMO_ADMIN_EMAIL and DEMO_ADMIN_PASSWORD (at least 12 characters) before initializing an empty demo database.");
        var admin = new AdminUser { FullName = "Demo Yöneticisi", Email = email };
        admin.Password = new PasswordHasher<AdminUser>().HashPassword(admin, password);
        context.AdminUsers.Add(admin);
        context.SaveChanges();
    }
}
