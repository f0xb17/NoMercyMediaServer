using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Translations_TranslationId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_TranslationId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "TranslationId",
                table: "People");

            migrationBuilder.CreateTable(
                name: "PersonTranslation",
                columns: table => new
                {
                    PeopleId = table.Column<int>(type: "INTEGER", nullable: false),
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTranslation", x => new { x.PeopleId, x.TranslationsId });
                    table.ForeignKey(
                        name: "FK_PersonTranslation_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonTranslation_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonTranslation_TranslationsId",
                table: "PersonTranslation",
                column: "TranslationsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonTranslation");

            migrationBuilder.AddColumn<int>(
                name: "TranslationId",
                table: "People",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_TranslationId",
                table: "People",
                column: "TranslationId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Translations_TranslationId",
                table: "People",
                column: "TranslationId",
                principalTable: "Translations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
