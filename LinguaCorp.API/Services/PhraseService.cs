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

        // Retrieve all phrases by language
        public List<Phrase> GetPhrasesByLanguage(string language)
        {
            try
            {
                return _context.Phrases.Where(p => p.Language == language).ToList();
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

        // Adds a single phrase
        public Phrase CreatePhrase(Phrase phrase)
        {
            _context.Phrases.Add(phrase);
            _context.SaveChanges();
            return phrase;
        }

        // Adds one or more phrases
        public List<Phrase> CreatePhrases(List<Phrase> phrases)
        {
            _context.Phrases.AddRange(phrases);
            _context.SaveChanges();
            return phrases;
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