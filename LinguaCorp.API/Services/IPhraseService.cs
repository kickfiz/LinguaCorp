using LinguaCorp.API.Models;

namespace LinguaCorp.API.Services
{
    public interface IPhraseService
    {
        public List<Phrase> GetAllPhrases();
        public Phrase? GetPhraseById(int id);
        public Phrase CreatePhrase(Phrase phrase);
    }
}