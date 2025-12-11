using LinguaCorp.API.Models;

namespace LinguaCorp.API.Services
{
    public interface IPhraseService
    {
        public List<Phrase> GetAllPhrases();
        public List<Phrase> GetPhrasesByLanguage(string language);
        public Phrase? GetPhraseById(int id);
        public Phrase CreatePhrase(Phrase phrase);
        public List<Phrase> CreatePhrases(List<Phrase> phrases);
        public bool UpdatePhrase(int id, Phrase updated);
        public bool DeletePhrase(int id);
    }
}