using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "Certifications",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Iso31661",
                table: "Certifications",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.CreateTable(
                name: "SpecialUser",
                columns: table => new
                {
                    SpecialId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialUser", x => new { x.SpecialId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SpecialUser_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserData_SpecialId",
                table: "UserData",
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialUser_UserId",
                table: "SpecialUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_Specials_SpecialId",
                table: "UserData",
                column: "SpecialId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_Specials_SpecialId",
                table: "UserData");

            migrationBuilder.DropTable(
                name: "SpecialUser");

            migrationBuilder.DropIndex(
                name: "IX_UserData_SpecialId",
                table: "UserData");

            migrationBuilder.AddColumn<string>(
                name: "SpecialId1",
                table: "UserData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "Certifications",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Iso31661",
                table: "Certifications",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

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
    }
}
