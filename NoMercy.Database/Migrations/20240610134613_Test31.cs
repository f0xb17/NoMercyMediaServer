using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Translations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AlbumId1",
                table: "Translations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "Translations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId1",
                table: "Translations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReleaseGroupId",
                table: "Translations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Artists",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleSort",
                table: "Artists",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Artists",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "AlbumId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId1",
                table: "Translations",
                column: "AlbumId1");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "ArtistId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId1",
                table: "Translations",
                column: "ArtistId1");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ReleaseGroupId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "ReleaseGroupId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Albums_AlbumId1",
                table: "Translations",
                column: "AlbumId1",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Artists_ArtistId1",
                table: "Translations",
                column: "ArtistId1",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_ReleaseGroups_ReleaseGroupId",
                table: "Translations",
                column: "ReleaseGroupId",
                principalTable: "ReleaseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Albums_AlbumId1",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Artists_ArtistId1",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_ReleaseGroups_ReleaseGroupId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId1",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId1",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ReleaseGroupId_Iso6391_Iso31661",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "AlbumId1",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ArtistId1",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ReleaseGroupId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "TitleSort",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Artists");
        }
    }
}
