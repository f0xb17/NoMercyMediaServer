using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Tracks",
                newName: "HostFolder");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_Filename_Path",
                table: "Tracks",
                newName: "IX_Tracks_Filename_HostFolder");

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "Artists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HostFolder",
                table: "Artists",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "Albums",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HostFolder",
                table: "Albums",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_FolderId",
                table: "Tracks",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_FolderId",
                table: "Artists",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_FolderId",
                table: "Albums",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Folders_FolderId",
                table: "Albums",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Folders_FolderId",
                table: "Artists",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Folders_FolderId",
                table: "Tracks",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Folders_FolderId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Folders_FolderId",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Folders_FolderId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_FolderId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Artists_FolderId",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Albums_FolderId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "HostFolder",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "HostFolder",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "HostFolder",
                table: "Tracks",
                newName: "Path");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_Filename_HostFolder",
                table: "Tracks",
                newName: "IX_Tracks_Filename_Path");
        }
    }
}
