using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EtkinlikAnketSistemi.Models
{
    public class SurveyQuestion
    {
        public int Id { get; set; }

        public int EventSurveyId { get; set; }

        [ValidateNever]
        public EventSurvey EventSurvey { get; set; } = null!;

        [Required(ErrorMessage = "Soru metni zorunludur.")]
        public string QuestionText { get; set; } = null!;

        [Required(ErrorMessage = "Soru tipi seçilmelidir.")]
        public string QuestionType { get; set; } = null!;

        [Range(1, 999, ErrorMessage = "Sıralama 1 veya daha büyük olmalıdır.")]
        public int DisplayOrder { get; set; }
    }
}