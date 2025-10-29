namespace LinguaCorp.API.Models
{
    public class Phrase
    {
        public int Id { get; set; }
        public string OriginalText { get; set; }
        public string Language { get; set; }
        public string TranslatedText { get; set; }
    }
}