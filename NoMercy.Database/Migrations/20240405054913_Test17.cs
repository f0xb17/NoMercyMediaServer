using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicPlays",
                table: "MusicPlays");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MusicPlays",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicPlays",
                table: "MusicPlays",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MusicPlays_UserId",
                table: "MusicPlays",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicPlays",
                table: "MusicPlays");

            migrationBuilder.DropIndex(
                name: "IX_MusicPlays_UserId",
                table: "MusicPlays");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MusicPlays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicPlays",
                table: "MusicPlays",
                columns: new[] { "UserId", "TrackId" });
        }
    }
}
