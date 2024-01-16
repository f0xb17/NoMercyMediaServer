using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_User_Users_UserId",
                table: "Album_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_Users_UserId",
                table: "Notification_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Track_User_Users_UserId",
                table: "Track_User");

            migrationBuilder.DropIndex(
                name: "IX_Track_User_UserId",
                table: "Track_User");

            migrationBuilder.DropIndex(
                name: "IX_Notification_User_UserId",
                table: "Notification_User");

            migrationBuilder.DropIndex(
                name: "IX_Album_User_UserId",
                table: "Album_User");

            migrationBuilder.AddColumn<bool>(
                name: "Allowed",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AudioTranscoding",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Manage",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "NoTranscoding",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Owner",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<bool>(
                name: "VideoTranscoding",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Track_User",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Notification_User",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Album_User",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Track_User_UserId1",
                table: "Track_User",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_User_UserId1",
                table: "Notification_User",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Album_User_UserId1",
                table: "Album_User",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_User_Users_UserId1",
                table: "Album_User",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_Library_Folders_FolderId",
                table: "Folder_Library",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_Users_UserId1",
                table: "Notification_User",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_User_Users_UserId1",
                table: "Track_User",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_User_Users_UserId1",
                table: "Album_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Folder_Library_Folders_FolderId",
                table: "Folder_Library");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_Users_UserId1",
                table: "Notification_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Track_User_Users_UserId1",
                table: "Track_User");

            migrationBuilder.DropIndex(
                name: "IX_Track_User_UserId1",
                table: "Track_User");

            migrationBuilder.DropIndex(
                name: "IX_Notification_User_UserId1",
                table: "Notification_User");

            migrationBuilder.DropIndex(
                name: "IX_Album_User_UserId1",
                table: "Album_User");

            migrationBuilder.DropColumn(
                name: "Allowed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AudioTranscoding",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Manage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NoTranscoding",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VideoTranscoding",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Track_User");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Notification_User");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Album_User");

            migrationBuilder.CreateIndex(
                name: "IX_Track_User_UserId",
                table: "Track_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_User_UserId",
                table: "Notification_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_User_UserId",
                table: "Album_User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_User_Users_UserId",
                table: "Album_User",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_Users_UserId",
                table: "Notification_User",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_User_Users_UserId",
                table: "Track_User",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
