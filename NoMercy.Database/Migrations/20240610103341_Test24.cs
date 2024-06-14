using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumRelease");

            migrationBuilder.DropTable(
                name: "ArtistRelease");

            migrationBuilder.CreateTable(
                name: "AlbumReleaseGroup",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReleaseGroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumReleaseGroup", x => new { x.AlbumId, x.ReleaseGroupId });
                    table.ForeignKey(
                        name: "FK_AlbumReleaseGroup_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumReleaseGroup_Releases_ReleaseGroupId",
                        column: x => x.ReleaseGroupId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistReleaseGroup",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReleaseGroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistReleaseGroup", x => new { x.ArtistId, x.ReleaseGroupId });
                    table.ForeignKey(
                        name: "FK_ArtistReleaseGroup_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistReleaseGroup_Releases_ReleaseGroupId",
                        column: x => x.ReleaseGroupId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenreReleaseGroup",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReleaseGroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenreReleaseGroup", x => new { x.GenreId, x.ReleaseGroupId });
                    table.ForeignKey(
                        name: "FK_MusicGenreReleaseGroup_MusicGenres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicGenreReleaseGroup_Releases_ReleaseGroupId",
                        column: x => x.ReleaseGroupId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumReleaseGroup_ReleaseGroupId",
                table: "AlbumReleaseGroup",
                column: "ReleaseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistReleaseGroup_ReleaseGroupId",
                table: "ArtistReleaseGroup",
                column: "ReleaseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenreReleaseGroup_ReleaseGroupId",
                table: "MusicGenreReleaseGroup",
                column: "ReleaseGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumReleaseGroup");

            migrationBuilder.DropTable(
                name: "ArtistReleaseGroup");

            migrationBuilder.DropTable(
                name: "MusicGenreReleaseGroup");

            migrationBuilder.CreateTable(
                name: "AlbumRelease",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumRelease", x => new { x.AlbumId, x.ReleaseId });
                    table.ForeignKey(
                        name: "FK_AlbumRelease_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumRelease_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistRelease",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistRelease", x => new { x.ArtistId, x.ReleaseId });
                    table.ForeignKey(
                        name: "FK_ArtistRelease_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistRelease_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumRelease_ReleaseId",
                table: "AlbumRelease",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistRelease_ReleaseId",
                table: "ArtistRelease",
                column: "ReleaseId");
        }
    }
}
