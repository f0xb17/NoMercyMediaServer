using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialItems_UserData_UserDataId",
                table: "SpecialItems");

            migrationBuilder.DropIndex(
                name: "IX_SpecialItems_UserDataId",
                table: "SpecialItems");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "SpecialItems");

            migrationBuilder.AddColumn<string>(
                name: "SpecialItemId",
                table: "UserData",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "EpisodeId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_SpecialItemId",
                table: "UserData",
                column: "SpecialItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserData_SpecialItems_SpecialItemId",
                table: "UserData",
                column: "SpecialItemId",
                principalTable: "SpecialItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserData_SpecialItems_SpecialItemId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_UserData_SpecialItemId",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "SpecialItemId",
                table: "UserData");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EpisodeId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDataId",
                table: "SpecialItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_UserDataId",
                table: "SpecialItems",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialItems_UserData_UserDataId",
                table: "SpecialItems",
                column: "UserDataId",
                principalTable: "UserData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
