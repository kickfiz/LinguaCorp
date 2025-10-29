using LinguaCorp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinguaCorp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : ControllerBase
    {
        // In-memory list of phrases
        private static List<Phrase> Phrases = new List<Phrase>
        {
            new Phrase { Id = 1, OriginalText = "Hello", Language = "en", TranslatedText = "Olá" },
            new Phrase { Id = 2, OriginalText = "Thank you", Language = "en", TranslatedText = "Obrigado" },
            new Phrase { Id = 3, OriginalText = "Good morning", Language = "en", TranslatedText = "Bom dia" }
        };

        // GET: api/phrases
        [HttpGet]
        public IActionResult GetAllPhrases()
        {
            return Ok(Phrases);
        }

        // GET: api/phrases/{id}
        [HttpGet("{id}")]
        public IActionResult GetPhraseById(int id)
        {
            var phrase = Phrases.FirstOrDefault(p => p.Id == id);
            return Ok(phrase);
        }
    }
}