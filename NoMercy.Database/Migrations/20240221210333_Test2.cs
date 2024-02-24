using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Libraries");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Libraries",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Libraries");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Libraries",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Libraries",
                type: "TEXT",
                maxLength: 256,
                nullable: true);
        }
    }
}
