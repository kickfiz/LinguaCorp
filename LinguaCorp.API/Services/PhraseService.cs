using LinguaCorp.API.Models;

namespace LinguaCorp.API.Services
{
    public class PhraseService : IPhraseService
    {
        // In-memory storage for simplicity (will be replaced by database later)
        private readonly List<Phrase> _phrases;

        public PhraseService()
        {
            // Initialize sample data
            _phrases = new List<Phrase>
            {
                new Phrase { Id = 1, OriginalText = "Hello", Language = "en", TranslatedText = "Olá" },
                new Phrase { Id = 2, OriginalText = "Thank you", Language = "en", TranslatedText = "Obrigado" },
                new Phrase { Id = 3, OriginalText = "Good morning", Language = "en", TranslatedText = "Bom dia" }
            };
        }

        // Retrieve all phrases
        public List<Phrase> GetAllPhrases() => _phrases;

        // Retrieve a phrase by ID
        public Phrase? GetPhraseById(int id)
        {
            return _phrases.FirstOrDefault(p => p.Id == id);
        }

        // Adds a new phrase
        public Phrase CreatePhrase(Phrase phrase)
        {
            // Assign a new unique ID to the phrase
            phrase.Id = _phrases.Any() ? _phrases.Max(p => p.Id) + 1 : 1;

            _phrases.Add(phrase);

            return phrase;
        }
    }
}