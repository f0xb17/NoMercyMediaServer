using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Specials_SpecialId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_SpecialId",
                table: "UserData");

            migrationBuilder.AddColumn<string>(
                name: "SpecialId1",
                table: "UserData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_SpecialId1",
                table: "UserData",
                column: "SpecialId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Specials_SpecialId1",
                table: "UserData",
                column: "SpecialId1",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Specials_SpecialId1",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_SpecialId1",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "SpecialId1",
                table: "UserData");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_SpecialId",
                table: "UserData",
                column: "SpecialId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Specials_SpecialId",
                table: "UserData",
                column: "SpecialId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
