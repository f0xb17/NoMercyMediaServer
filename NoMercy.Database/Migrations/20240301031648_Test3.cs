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
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Episodes_EpisodeId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Movies_MovieId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_People_PersonId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Seasons_SeasonId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Tvs_TvId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_VideoFiles_VideoFileId",
                table: "Media");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "Medias");

            migrationBuilder.RenameIndex(
                name: "IX_Media_VideoFileId_Src",
                table: "Medias",
                newName: "IX_Medias_VideoFileId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Media_TvId_Src",
                table: "Medias",
                newName: "IX_Medias_TvId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Media_SeasonId_Src",
                table: "Medias",
                newName: "IX_Medias_SeasonId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Media_PersonId_Src",
                table: "Medias",
                newName: "IX_Medias_PersonId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Media_MovieId_Src",
                table: "Medias",
                newName: "IX_Medias_MovieId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Media_EpisodeId_Src",
                table: "Medias",
                newName: "IX_Medias_EpisodeId_Src");

            migrationBuilder.AlterColumn<string>(
                name: "Share",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Quality",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Languages",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HostFolder",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tvs",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Src",
                table: "Medias",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceId",
                table: "Devices",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Episodes_EpisodeId",
                table: "Medias",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Movies_MovieId",
                table: "Medias",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_People_PersonId",
                table: "Medias",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Seasons_SeasonId",
                table: "Medias",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Tvs_TvId",
                table: "Medias",
                column: "TvId",
                principalTable: "Tvs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_VideoFiles_VideoFileId",
                table: "Medias",
                column: "VideoFileId",
                principalTable: "VideoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Episodes_EpisodeId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Movies_MovieId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_People_PersonId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Seasons_SeasonId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Tvs_TvId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Medias_VideoFiles_VideoFileId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceId",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "Media");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_VideoFileId_Src",
                table: "Media",
                newName: "IX_Media_VideoFileId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_TvId_Src",
                table: "Media",
                newName: "IX_Media_TvId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_SeasonId_Src",
                table: "Media",
                newName: "IX_Media_SeasonId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_PersonId_Src",
                table: "Media",
                newName: "IX_Media_PersonId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_MovieId_Src",
                table: "Media",
                newName: "IX_Media_MovieId_Src");

            migrationBuilder.RenameIndex(
                name: "IX_Medias_EpisodeId_Src",
                table: "Media",
                newName: "IX_Media_EpisodeId_Src");

            migrationBuilder.AlterColumn<string>(
                name: "Share",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Quality",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Languages",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "HostFolder",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tvs",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Src",
                table: "Media",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Episodes_EpisodeId",
                table: "Media",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Movies_MovieId",
                table: "Media",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_People_PersonId",
                table: "Media",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Seasons_SeasonId",
                table: "Media",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Tvs_TvId",
                table: "Media",
                column: "TvId",
                principalTable: "Tvs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_VideoFiles_VideoFileId",
                table: "Media",
                column: "VideoFileId",
                principalTable: "VideoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
