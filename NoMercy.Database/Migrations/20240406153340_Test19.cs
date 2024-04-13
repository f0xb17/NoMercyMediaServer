using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EpisodeId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialId",
                table: "SpecialItems",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserDataId",
                table: "SpecialItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_MovieId",
                table: "SpecialItems",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_SpecialId_EpisodeId",
                table: "SpecialItems",
                columns: new[] { "SpecialId", "EpisodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_SpecialId_MovieId",
                table: "SpecialItems",
                columns: new[] { "SpecialId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_UserDataId",
                table: "SpecialItems",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialItems_Movies_MovieId",
                table: "SpecialItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialItems_Specials_SpecialId",
                table: "SpecialItems",
                column: "SpecialId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialItems_UserData_UserDataId",
                table: "SpecialItems",
                column: "UserDataId",
                principalTable: "UserData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialItems_Movies_MovieId",
                table: "SpecialItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialItems_Specials_SpecialId",
                table: "SpecialItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialItems_UserData_UserDataId",
                table: "SpecialItems");

            migrationBuilder.DropIndex(
                name: "IX_SpecialItems_MovieId",
                table: "SpecialItems");

            migrationBuilder.DropIndex(
                name: "IX_SpecialItems_SpecialId_EpisodeId",
                table: "SpecialItems");

            migrationBuilder.DropIndex(
                name: "IX_SpecialItems_SpecialId_MovieId",
                table: "SpecialItems");

            migrationBuilder.DropIndex(
                name: "IX_SpecialItems_UserDataId",
                table: "SpecialItems");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "SpecialItems");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SpecialItems");

            migrationBuilder.DropColumn(
                name: "SpecialId",
                table: "SpecialItems");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "SpecialItems");

            migrationBuilder.AlterColumn<int>(
                name: "EpisodeId",
                table: "SpecialItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
