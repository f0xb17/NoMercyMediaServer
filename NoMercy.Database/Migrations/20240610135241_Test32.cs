using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Albums_AlbumId1",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Artists_ArtistId1",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId1",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId1",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "AlbumId1",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ArtistId1",
                table: "Translations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ArtistId",
                table: "Translations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AlbumId",
                table: "Translations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Albums_AlbumId",
                table: "Translations",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Artists_ArtistId",
                table: "Translations",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Albums_AlbumId",
                table: "Translations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Artists_ArtistId",
                table: "Translations");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "Translations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Translations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AlbumId1",
                table: "Translations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId1",
                table: "Translations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId1",
                table: "Translations",
                column: "AlbumId1");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId1",
                table: "Translations",
                column: "ArtistId1");

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
        }
    }
}
