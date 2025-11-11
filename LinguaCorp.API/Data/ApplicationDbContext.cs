using LinguaCorp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguaCorp.API.Data;

/// <summary>
/// Database context for LinguaCorp Translation API
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Phrases collection
    /// </summary>
    public DbSet<Phrase> Phrases { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<Phrase>().HasData(
            new Phrase
            {
                Id = 1,
                OriginalText = "Hello",
                Language = "en",
                TranslatedText = "Olá"
            },
            new Phrase
            {
                Id = 2,
                OriginalText = "Thank you",
                Language = "en",
                TranslatedText = "Obrigado"
            },
            new Phrase
            {
                Id = 3,
                OriginalText = "Good morning",
                Language = "en",
                TranslatedText = "Bom dia"
            }
        );
    }
}
