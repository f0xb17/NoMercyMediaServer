using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumReleaseGroup_Releases_ReleaseGroupId",
                table: "AlbumReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistReleaseGroup_Releases_ReleaseGroupId",
                table: "ArtistReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicGenreReleaseGroup_Releases_ReleaseGroupId",
                table: "MusicGenreReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Releases_Libraries_LibraryId",
                table: "Releases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Releases",
                table: "Releases");

            migrationBuilder.RenameTable(
                name: "Releases",
                newName: "ReleaseGroups");

            migrationBuilder.RenameIndex(
                name: "IX_Releases_LibraryId",
                table: "ReleaseGroups",
                newName: "IX_ReleaseGroups_LibraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReleaseGroups",
                table: "ReleaseGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "AlbumReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "ReleaseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "ArtistReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "ReleaseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicGenreReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "MusicGenreReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "ReleaseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReleaseGroups_Libraries_LibraryId",
                table: "ReleaseGroups",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "AlbumReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "ArtistReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicGenreReleaseGroup_ReleaseGroups_ReleaseGroupId",
                table: "MusicGenreReleaseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ReleaseGroups_Libraries_LibraryId",
                table: "ReleaseGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReleaseGroups",
                table: "ReleaseGroups");

            migrationBuilder.RenameTable(
                name: "ReleaseGroups",
                newName: "Releases");

            migrationBuilder.RenameIndex(
                name: "IX_ReleaseGroups_LibraryId",
                table: "Releases",
                newName: "IX_Releases_LibraryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Releases",
                table: "Releases",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumReleaseGroup_Releases_ReleaseGroupId",
                table: "AlbumReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistReleaseGroup_Releases_ReleaseGroupId",
                table: "ArtistReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicGenreReleaseGroup_Releases_ReleaseGroupId",
                table: "MusicGenreReleaseGroup",
                column: "ReleaseGroupId",
                principalTable: "Releases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Releases_Libraries_LibraryId",
                table: "Releases",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
