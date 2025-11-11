using LinguaCorp.API.Data;
using LinguaCorp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguaCorp.API.Services
{
    public class PhraseService : IPhraseService
    {
        private readonly ApplicationDbContext _context;

        public PhraseService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all phrases
        public List<Phrase> GetAllPhrases()
        {
            try
            {
                return _context.Phrases.ToList();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Phrase data could not be retrieved.");
            }
        }

        // Retrieve a phrase by ID
        public Phrase GetPhraseById(int id)
        {
            var phrase = _context.Phrases.Find(id);

            if (phrase == null)
            {
                throw new KeyNotFoundException($"Phrase with ID {id} not found.");
            }

            return phrase;
        }

        // Adds a new phrase
        public Phrase CreatePhrase(Phrase phrase)
        {
            _context.Phrases.Add(phrase);
            _context.SaveChanges();

            return phrase;
        }

        // Replaces an existing phrase
        public bool UpdatePhrase(int id, Phrase updated)
        {
            var existingPhrase = _context.Phrases.Find(id);

            if (existingPhrase == null)
            {
                return false;
            }

            // Update fields of the existing phrase
            existingPhrase.OriginalText = updated.OriginalText;
            existingPhrase.Language = updated.Language;
            existingPhrase.TranslatedText = updated.TranslatedText;

            _context.SaveChanges();

            return true;
        }

        // Removes a Phrase
        public bool DeletePhrase(int id)
        {
            var phrase = _context.Phrases.Find(id);
            if (phrase == null)
            {
                return false;
            }

            _context.Phrases.Remove(phrase);
            _context.SaveChanges();
            return true;
        }
    }
}