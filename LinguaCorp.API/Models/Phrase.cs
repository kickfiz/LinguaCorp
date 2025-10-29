using System.ComponentModel.DataAnnotations;

namespace LinguaCorp.API.Models
{
    public class Phrase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "OriginalText is required.")]
        public string OriginalText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Language must be a 2-letter ISO code.")]
        public string Language { get; set; } = string.Empty;

        public string TranslatedText { get; set; } = string.Empty;
    }
}