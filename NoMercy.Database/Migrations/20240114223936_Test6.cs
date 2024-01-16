using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "Parameters",
                table: "Jobs",
                newName: "Queue");

            migrationBuilder.RenameColumn(
                name: "MethodName",
                table: "Jobs",
                newName: "Payload");

            migrationBuilder.RenameColumn(
                name: "StackTrace",
                table: "FailedJobs",
                newName: "Queue");

            migrationBuilder.RenameColumn(
                name: "Parameters",
                table: "FailedJobs",
                newName: "Payload");

            migrationBuilder.RenameColumn(
                name: "MethodName",
                table: "FailedJobs",
                newName: "Exception");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "FailedJobs",
                newName: "Connection");

            migrationBuilder.AddColumn<byte>(
                name: "Attempts",
                table: "Jobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableAt",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservedAt",
                table: "Jobs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedAt",
                table: "FailedJobs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "FailedJobs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AvailableAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ReservedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FailedAt",
                table: "FailedJobs");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "FailedJobs");

            migrationBuilder.RenameColumn(
                name: "Queue",
                table: "Jobs",
                newName: "Parameters");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "Jobs",
                newName: "MethodName");

            migrationBuilder.RenameColumn(
                name: "Queue",
                table: "FailedJobs",
                newName: "StackTrace");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "FailedJobs",
                newName: "Parameters");

            migrationBuilder.RenameColumn(
                name: "Exception",
                table: "FailedJobs",
                newName: "MethodName");

            migrationBuilder.RenameColumn(
                name: "Connection",
                table: "FailedJobs",
                newName: "ClassName");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Jobs",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
