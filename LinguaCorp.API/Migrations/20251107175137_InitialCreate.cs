using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LinguaCorp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phrases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalText = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    TranslatedText = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrases", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Phrases",
                columns: new[] { "Id", "Language", "OriginalText", "TranslatedText" },
                values: new object[,]
                {
                    { 1, "en", "Hello", "Olá" },
                    { 2, "en", "Thank you", "Obrigado" },
                    { 3, "en", "Good morning", "Bom dia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phrases");
        }
    }
}
