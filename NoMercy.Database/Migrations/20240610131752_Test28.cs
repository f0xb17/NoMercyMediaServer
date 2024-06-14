using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ReleaseGroups",
                newName: "Disambiguation");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Artists",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Albums",
                type: "TEXT",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "Disambiguation",
                table: "ReleaseGroups",
                newName: "Description");
        }
    }
}
