using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Artists",
                newName: "Disambiguation");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Albums",
                newName: "Disambiguation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Disambiguation",
                table: "Artists",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Disambiguation",
                table: "Albums",
                newName: "Description");
        }
    }
}
