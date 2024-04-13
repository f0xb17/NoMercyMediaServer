using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "UserData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_CollectionId",
                table: "UserData",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Collections_CollectionId",
                table: "UserData",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Collections_CollectionId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_CollectionId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "UserData");
        }
    }
}
