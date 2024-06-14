using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ReleaseGroupId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId_Iso31661",
                table: "Translations",
                columns: new[] { "AlbumId", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId_Iso31661",
                table: "Translations",
                columns: new[] { "ArtistId", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ReleaseGroupId_Iso31661",
                table: "Translations",
                columns: new[] { "ReleaseGroupId", "Iso31661" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ReleaseGroupId_Iso31661",
                table: "Translations");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "AlbumId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "ArtistId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ReleaseGroupId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "ReleaseGroupId", "Iso6391", "Iso31661" },
                unique: true);
        }
    }
}
