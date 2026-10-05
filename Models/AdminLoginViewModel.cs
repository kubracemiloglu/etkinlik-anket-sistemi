using System.ComponentModel.DataAnnotations;

namespace EtkinlikAnketSistemi.Models
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        public string Password { get; set; } = null!;
    }
}