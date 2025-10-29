using LinguaCorp.API.Models;
using LinguaCorp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinguaCorp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : ControllerBase
    {
        private readonly IPhraseService _phraseService;

        // The service is automatically injected via dependency injection
        public PhrasesController(IPhraseService phraseService)
        {
            _phraseService = phraseService;
        }

        [HttpGet]
        public IActionResult GetAllPhrases()
        {
            var phrases = _phraseService.GetAllPhrases();

            if (phrases.Count == 0)
            {
                return NoContent();
            }

            return Ok(phrases);
        }

        [HttpGet("{id}")]
        public IActionResult GetPhraseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be a positive integer.");
            }

            var phrase = _phraseService.GetPhraseById(id);

            if (phrase == null)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            return Ok(phrase);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Phrase phrase)
        {
            // This verifies if the Model is filled correctly. If not, the error message is returned
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = _phraseService.CreatePhrase(phrase);

            return CreatedAtAction(nameof(GetPhraseById), new { id = created.Id }, created);
        }
    }
}