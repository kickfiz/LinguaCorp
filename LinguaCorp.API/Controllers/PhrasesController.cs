using LinguaCorp.API.Models;
using LinguaCorp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinguaCorp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : ControllerBase
    {
        private readonly IPhraseService _phraseService;
        private readonly ILogger<PhrasesController> _logger;

        // Injected via dependency injection
        public PhrasesController(ILogger<PhrasesController> logger, IPhraseService phraseService)
        {
            _logger = logger;
            _phraseService = phraseService;
        }

        /// <summary>
        /// Retrieves all stored phrases.
        /// </summary>
        /// <returns>List of phrases or 204 if empty.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllPhrases()
        {
            _logger.LogInformation("Request received to get all phrases");

            try
            {
                var phrases = _phraseService.GetAllPhrases();

                if (phrases.Count == 0)
                {
                    _logger.LogInformation("No phrases found in the system");
                    return NoContent();
                }

                _logger.LogInformation("Retrieved {Count} phrases successfully", phrases.Count);
                return Ok(phrases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all phrases.");
                return StatusCode(500, "An error occurred while retrieving phrases.");
            }
        }

        /// <summary>
        /// Retrieves a specific phrase by its unique identifier.
        /// </summary>
        /// <param name="id">Phrase ID</param>
        /// <returns>Returns the phrase if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPhraseById(int id)
        {
            _logger.LogInformation("Request received to get phrase with ID {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID {Id} provided", id);
                return BadRequest("ID must be a positive integer.");
            }

            try
            {
                var phrase = _phraseService.GetPhraseById(id);

                _logger.LogInformation("Phrase with ID {Id} retrieved successfully", id);
                return Ok(phrase);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving phrase with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the phrase.");
            }
        }

        /// <summary>
        /// Creates a new phrase.
        /// </summary>
        /// <param name="phrase">Phrase object to create</param>
        /// <returns>Returns the created phrase with assigned ID.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] Phrase phrase)
        {
            _logger.LogInformation("Request received to create a new phrase");

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for creating phrase");
                    return BadRequest(ModelState);
                }

                var created = _phraseService.CreatePhrase(phrase);

                _logger.LogInformation("Phrase created successfully with ID {Id}", created.Id);
                return CreatedAtAction(nameof(GetPhraseById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating phrase");
                return StatusCode(500, "An error occurred while creating the phrase.");
            }
        }

        /// <summary>
        /// Updates an existing phrase.
        /// </summary>
        /// <param name="id">ID of the phrase to update</param>
        /// <param name="updatedPhrase">Updated phrase data</param>
        /// <returns>Returns the updated phrase.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePhrase(int id, [FromBody] Phrase updatedPhrase)
        {
            _logger.LogInformation("Request received to update phrase with ID {Id}", id);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for updating phrase with ID {Id}", id);
                    return BadRequest(ModelState);
                }

                var success = _phraseService.UpdatePhrase(id, updatedPhrase);

                if (!success)
                {
                    _logger.LogWarning("Phrase with ID {Id} not found for update", id);
                    return NotFound($"Phrase with ID {id} not found.");
                }

                _logger.LogInformation("Phrase with ID {Id} updated successfully", id);
                return Ok(updatedPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating phrase with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the phrase.");
            }
        }

        /// <summary>
        /// Deletes a phrase by ID.
        /// </summary>
        /// <param name="id">ID of the phrase to delete</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePhrase(int id)
        {
            _logger.LogInformation("Request received to delete phrase with ID {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID {Id} provided for deletion", id);
                return BadRequest("ID must be a positive integer.");
            }

            try
            {
                var success = _phraseService.DeletePhrase(id);

                if (!success)
                {
                    _logger.LogWarning("Phrase with ID {Id} not found for deletion", id);
                    return NotFound($"Phrase with ID {id} not found.");
                }

                _logger.LogInformation("Phrase with ID {Id} deleted successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting phrase with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the phrase.");
            }
        }
    }
}