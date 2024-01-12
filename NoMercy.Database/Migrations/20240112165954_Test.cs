using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso31661 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Rating = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Meaning = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso31661 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NativeName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Browser = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Os = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Device = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CustomName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Version = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Ip = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EncoderProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Container = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Param = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncoderProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso6391 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    AutoRefreshInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    ChapterImages = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExtractChapters = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExtractChaptersDuring = table.Column<bool>(type: "INTEGER", nullable: false),
                    Image = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    PerfectSubtitleMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    Realtime = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpecialSeasonName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Language = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaStreams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStreams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenres",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunningTasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunningTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Cover = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Tracks = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Cover = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Parts = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collections_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EncoderProfile_Library",
                columns: table => new
                {
                    EncoderProfileId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncoderProfile_Library", x => new { x.EncoderProfileId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_EncoderProfile_Library_EncoderProfiles_EncoderProfileId",
                        column: x => x.EncoderProfileId,
                        principalTable: "EncoderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncoderProfile_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folder_Library",
                columns: table => new
                {
                    FolderId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder_Library", x => new { x.FolderId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Folder_Library_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folder_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Language_Library",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LanguageId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language_Library", x => new { x.LanguageId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Language_Library_Languages_LanguageId1",
                        column: x => x.LanguageId1,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Language_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Duration = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Show = table.Column<bool>(type: "INTEGER", nullable: false),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Adult = table.Column<bool>(type: "INTEGER", nullable: false),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Budget = table.Column<int>(type: "INTEGER", nullable: true),
                    Homepage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ImdbId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OriginalTitle = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OriginalLanguage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Popularity = table.Column<double>(type: "REAL", nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Revenue = table.Column<int>(type: "INTEGER", nullable: true),
                    Runtime = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Tagline = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Trailer = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    MoviedbId = table.Column<int>(type: "INTEGER", nullable: true),
                    Video = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    VoteAverage = table.Column<double>(type: "REAL", nullable: true),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: true),
                    BlurHash = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tvs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    HaveEpisodes = table.Column<int>(type: "INTEGER", nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Duration = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstAirDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Homepage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ImdbId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    InProduction = table.Column<bool>(type: "INTEGER", nullable: true),
                    LastEpisodeToAir = table.Column<int>(type: "INTEGER", nullable: true),
                    MediaType = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NextEpisodeToAir = table.Column<int>(type: "INTEGER", nullable: true),
                    NumberOfEpisodes = table.Column<int>(type: "INTEGER", nullable: true),
                    NumberOfSeasons = table.Column<int>(type: "INTEGER", nullable: true),
                    OriginCountry = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OriginalLanguage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Popularity = table.Column<double>(type: "REAL", nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    SpokenLanguages = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Tagline = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Trailer = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TvdbId = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    VoteAverage = table.Column<double>(type: "REAL", nullable: true),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tvs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tvs_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Priority_Provider",
                columns: table => new
                {
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ProviderId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority_Provider", x => new { x.Priority, x.Country, x.ProviderId });
                    table.ForeignKey(
                        name: "FK_Priority_Provider_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Library_Track",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library_Track", x => new { x.LibraryId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_Library_Track_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Library_Track_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenre_Track",
                columns: table => new
                {
                    GenreId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    GenreId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenre_Track", x => new { x.GenreId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_MusicGenre_Track_Genres_GenreId1",
                        column: x => x.GenreId1,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicGenre_Track_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlist_Track",
                columns: table => new
                {
                    PlaylistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlist_Track", x => new { x.PlaylistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_Playlist_Track_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Playlist_Track_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Library_User",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library_User", x => new { x.LibraryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Library_User_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Library_User_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Music_Plays",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Music_Plays", x => new { x.UserId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_Music_Plays_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Music_Plays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification_User",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification_User", x => new { x.NotificationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Notification_User_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_User_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Track_User",
                columns: table => new
                {
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track_User", x => new { x.TrackId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Track_User_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Track_User_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album_Library",
                columns: table => new
                {
                    AlbumId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album_Library", x => new { x.AlbumId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Album_Library_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album_MusicGenre",
                columns: table => new
                {
                    AlbumId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MusicGenreId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album_MusicGenre", x => new { x.AlbumId, x.MusicGenreId });
                    table.ForeignKey(
                        name: "FK_Album_MusicGenre_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_MusicGenre_MusicGenres_MusicGenreId",
                        column: x => x.MusicGenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album_Track",
                columns: table => new
                {
                    AlbumId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album_Track", x => new { x.AlbumId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_Album_Track_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_Track_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album_User",
                columns: table => new
                {
                    AlbumId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album_User", x => new { x.AlbumId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Album_User_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_User_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album_Artist",
                columns: table => new
                {
                    AlbumId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ArtistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album_Artist", x => new { x.AlbumId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_Album_Artist_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_Artist_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artist_Library",
                columns: table => new
                {
                    ArtistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist_Library", x => new { x.ArtistId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Artist_Library_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Artist_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artist_MusicGenre",
                columns: table => new
                {
                    ArtistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MusicGenreId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist_MusicGenre", x => new { x.ArtistId, x.MusicGenreId });
                    table.ForeignKey(
                        name: "FK_Artist_MusicGenre_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Artist_MusicGenre_MusicGenres_MusicGenreId",
                        column: x => x.MusicGenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artist_Track",
                columns: table => new
                {
                    ArtistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TrackId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist_Track", x => new { x.ArtistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_Artist_Track_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Artist_Track_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artist_User",
                columns: table => new
                {
                    ArtistId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist_User", x => new { x.ArtistId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Artist_User_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Artist_User_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collection_Library",
                columns: table => new
                {
                    CollectionId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CollectionId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection_Library", x => new { x.CollectionId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Collection_Library_Collections_CollectionId1",
                        column: x => x.CollectionId1,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collection_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certification_Movie",
                columns: table => new
                {
                    CertificationId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    CertificationId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certification_Movie", x => new { x.CertificationId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Certification_Movie_Certifications_CertificationId1",
                        column: x => x.CertificationId1,
                        principalTable: "Certifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certification_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collection_Movie",
                columns: table => new
                {
                    CollectionId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    CollectionId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collection_Movie", x => new { x.CollectionId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Collection_Movie_Collections_CollectionId1",
                        column: x => x.CollectionId1,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collection_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Genre_Movie",
                columns: table => new
                {
                    GenreId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre_Movie", x => new { x.GenreId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Genre_Movie_Genres_GenreId1",
                        column: x => x.GenreId1,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Genre_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keyword_Movie",
                columns: table => new
                {
                    KeywordId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    KeywordId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword_Movie", x => new { x.KeywordId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Keyword_Movie_Keywords_KeywordId1",
                        column: x => x.KeywordId1,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Keyword_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Library_Movie",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library_Movie", x => new { x.LibraryId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_Library_Movie_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Library_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso31661 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternativeTitles_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlternativeTitles_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certification_Tv",
                columns: table => new
                {
                    CertificationId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false),
                    CertificationId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certification_Tv", x => new { x.CertificationId, x.TvId });
                    table.ForeignKey(
                        name: "FK_Certification_Tv_Certifications_CertificationId1",
                        column: x => x.CertificationId1,
                        principalTable: "Certifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certification_Tv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Genre_Tv",
                columns: table => new
                {
                    GenreId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre_Tv", x => new { x.GenreId, x.TvId });
                    table.ForeignKey(
                        name: "FK_Genre_Tv_Genres_GenreId1",
                        column: x => x.GenreId1,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Genre_Tv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keyword_Tv",
                columns: table => new
                {
                    KeywordId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false),
                    KeywordId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword_Tv", x => new { x.KeywordId, x.TvId });
                    table.ForeignKey(
                        name: "FK_Keyword_Tv_Keywords_KeywordId1",
                        column: x => x.KeywordId1,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Keyword_Tv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Library_Tv",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library_Tv", x => new { x.LibraryId, x.TvId });
                    table.ForeignKey(
                        name: "FK_Library_Tv_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Library_Tv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommendations_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Movies_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Tvs_TvId1",
                        column: x => x.TvId1,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    AirDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EpisodeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    SeasonNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seasons_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Similar",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Similar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Similar_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Movies_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Tvs_TvId1",
                        column: x => x.TvId1,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserData_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    AirDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EpisodeNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ImdbId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ProductionCode = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    SeasonNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Still = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TvdbId = table.Column<int>(type: "INTEGER", nullable: true),
                    VoteAverage = table.Column<float>(type: "REAL", nullable: true),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Episodes_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Casts_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Casts_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Casts_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Casts_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Crews_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crews_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crews_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crews_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestStars",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestStars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestStars_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestStars_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medias_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medias_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medias_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialItems_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso31661 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Iso6391 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Homepage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Biography = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translations_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Translations_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoFiles_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Adult = table.Column<bool>(type: "INTEGER", nullable: false),
                    AlsoKnownAs = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Biography = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    BirthDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeathDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Homepage = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ImdbId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    KnownForDepartment = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Popularity = table.Column<float>(type: "REAL", nullable: true),
                    Profile = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CastsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CrewsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Casts_CastsId",
                        column: x => x.CastsId,
                        principalTable: "Casts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Crews_CrewsId",
                        column: x => x.CrewsId,
                        principalTable: "Crews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File_Library",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File_Library", x => new { x.FileId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_File_Library_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_File_Library_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File_Movie",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File_Movie", x => new { x.FileId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_File_Movie_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_File_Movie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Creators",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => new { x.PersonId, x.TvId });
                    table.ForeignKey(
                        name: "FK_Creators_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Creators_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_DeviceId",
                table: "ActivityLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Artist_ArtistId",
                table: "Album_Artist",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Library_LibraryId",
                table: "Album_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_MusicGenre_MusicGenreId",
                table: "Album_MusicGenre",
                column: "MusicGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_Track_TrackId",
                table: "Album_Track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_User_UserId",
                table: "Album_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_LibraryId",
                table: "Albums",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_MovieId",
                table: "AlternativeTitles",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_Title",
                table: "AlternativeTitles",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_TvId",
                table: "AlternativeTitles",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Artist_Library_LibraryId",
                table: "Artist_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Artist_MusicGenre_MusicGenreId",
                table: "Artist_MusicGenre",
                column: "MusicGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Artist_Track_TrackId",
                table: "Artist_Track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Artist_User_UserId",
                table: "Artist_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_LibraryId",
                table: "Artists",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_EpisodeId",
                table: "Casts",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_Id_EpisodeId",
                table: "Casts",
                columns: new[] { "Id", "EpisodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_Id_MovieId",
                table: "Casts",
                columns: new[] { "Id", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_Id_SeasonId",
                table: "Casts",
                columns: new[] { "Id", "SeasonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_Id_TvId",
                table: "Casts",
                columns: new[] { "Id", "TvId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_MovieId",
                table: "Casts",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_SeasonId",
                table: "Casts",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_TvId",
                table: "Casts",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Certification_Movie_CertificationId1",
                table: "Certification_Movie",
                column: "CertificationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Certification_Movie_MovieId",
                table: "Certification_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Certification_Tv_CertificationId1",
                table: "Certification_Tv",
                column: "CertificationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Certification_Tv_TvId",
                table: "Certification_Tv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_Iso31661_Rating",
                table: "Certifications",
                columns: new[] { "Iso31661", "Rating" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collection_Library_CollectionId1",
                table: "Collection_Library",
                column: "CollectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_Library_LibraryId",
                table: "Collection_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_Movie_CollectionId1",
                table: "Collection_Movie",
                column: "CollectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Collection_Movie_MovieId",
                table: "Collection_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_LibraryId",
                table: "Collections",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Iso31661",
                table: "Countries",
                column: "Iso31661",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Creators_TvId",
                table: "Creators",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_EpisodeId",
                table: "Crews",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Id_EpisodeId",
                table: "Crews",
                columns: new[] { "Id", "EpisodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Id_MovieId",
                table: "Crews",
                columns: new[] { "Id", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Id_SeasonId",
                table: "Crews",
                columns: new[] { "Id", "SeasonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_Id_TvId",
                table: "Crews",
                columns: new[] { "Id", "TvId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_MovieId",
                table: "Crews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_SeasonId",
                table: "Crews",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_TvId",
                table: "Crews",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_EncoderProfile_Library_LibraryId",
                table: "EncoderProfile_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeasonId",
                table: "Episodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_TvId",
                table: "Episodes",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_File_Library_LibraryId",
                table: "File_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_File_Movie_MovieId",
                table: "File_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_EpisodeId",
                table: "Files",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_Library_LibraryId",
                table: "Folder_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Movie_GenreId1",
                table: "Genre_Movie",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Movie_MovieId",
                table: "Genre_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Tv_GenreId1",
                table: "Genre_Tv",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Tv_TvId",
                table: "Genre_Tv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_EpisodeId",
                table: "GuestStars",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_SeasonId",
                table: "GuestStars",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_EpisodeId",
                table: "Images",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_MovieId",
                table: "Images",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SeasonId",
                table: "Images",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TvId",
                table: "Images",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_Movie_KeywordId1",
                table: "Keyword_Movie",
                column: "KeywordId1");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_Movie_MovieId",
                table: "Keyword_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_Tv_KeywordId1",
                table: "Keyword_Tv",
                column: "KeywordId1");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_Tv_TvId",
                table: "Keyword_Tv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_Library_LanguageId1",
                table: "Language_Library",
                column: "LanguageId1");

            migrationBuilder.CreateIndex(
                name: "IX_Language_Library_LibraryId",
                table: "Language_Library",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Iso6391",
                table: "Languages",
                column: "Iso6391",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Library_Movie_MovieId",
                table: "Library_Movie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Library_Track_TrackId",
                table: "Library_Track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Library_Tv_TvId",
                table: "Library_Tv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Library_User_UserId",
                table: "Library_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_EpisodeId",
                table: "Medias",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_MovieId",
                table: "Medias",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_SeasonId",
                table: "Medias",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_TvId",
                table: "Medias",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_LibraryId",
                table: "Movies",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Music_Plays_TrackId",
                table: "Music_Plays",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenre_Track_GenreId1",
                table: "MusicGenre_Track",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenre_Track_TrackId",
                table: "MusicGenre_Track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_User_UserId",
                table: "Notification_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CastsId",
                table: "People",
                column: "CastsId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CrewsId",
                table: "People",
                column: "CrewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_Track_TrackId",
                table: "Playlist_Track",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Priority_Provider_ProviderId",
                table: "Priority_Provider",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MovieId",
                table: "Recommendations",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MovieId1",
                table: "Recommendations",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TvId",
                table: "Recommendations",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TvId1",
                table: "Recommendations",
                column: "TvId1");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_MovieId",
                table: "Seasons",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_TvId",
                table: "Seasons",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MovieId",
                table: "Similar",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MovieId1",
                table: "Similar",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_TvId",
                table: "Similar",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_TvId1",
                table: "Similar",
                column: "TvId1");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_EpisodeId",
                table: "SpecialItems",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Track_User_UserId",
                table: "Track_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_CollectionId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "CollectionId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_EpisodeId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "EpisodeId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_MovieId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "MovieId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_PersonId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "PersonId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_SeasonId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "SeasonId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translations_TvId_Iso6391_Iso31661",
                table: "Translations",
                columns: new[] { "TvId", "Iso6391", "Iso31661" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tvs_LibraryId",
                table: "Tvs",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_MovieId",
                table: "UserData",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_TvId",
                table: "UserData",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoFiles_EpisodeId",
                table: "VideoFiles",
                column: "EpisodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Album_Artist");

            migrationBuilder.DropTable(
                name: "Album_Library");

            migrationBuilder.DropTable(
                name: "Album_MusicGenre");

            migrationBuilder.DropTable(
                name: "Album_Track");

            migrationBuilder.DropTable(
                name: "Album_User");

            migrationBuilder.DropTable(
                name: "AlternativeTitles");

            migrationBuilder.DropTable(
                name: "Artist_Library");

            migrationBuilder.DropTable(
                name: "Artist_MusicGenre");

            migrationBuilder.DropTable(
                name: "Artist_Track");

            migrationBuilder.DropTable(
                name: "Artist_User");

            migrationBuilder.DropTable(
                name: "Certification_Movie");

            migrationBuilder.DropTable(
                name: "Certification_Tv");

            migrationBuilder.DropTable(
                name: "Collection_Library");

            migrationBuilder.DropTable(
                name: "Collection_Movie");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Creators");

            migrationBuilder.DropTable(
                name: "EncoderProfile_Library");

            migrationBuilder.DropTable(
                name: "File_Library");

            migrationBuilder.DropTable(
                name: "File_Movie");

            migrationBuilder.DropTable(
                name: "Folder_Library");

            migrationBuilder.DropTable(
                name: "Genre_Movie");

            migrationBuilder.DropTable(
                name: "Genre_Tv");

            migrationBuilder.DropTable(
                name: "GuestStars");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Keyword_Movie");

            migrationBuilder.DropTable(
                name: "Keyword_Tv");

            migrationBuilder.DropTable(
                name: "Language_Library");

            migrationBuilder.DropTable(
                name: "Library_Movie");

            migrationBuilder.DropTable(
                name: "Library_Track");

            migrationBuilder.DropTable(
                name: "Library_Tv");

            migrationBuilder.DropTable(
                name: "Library_User");

            migrationBuilder.DropTable(
                name: "MediaAttachments");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "MediaStreams");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "Music_Plays");

            migrationBuilder.DropTable(
                name: "MusicGenre_Track");

            migrationBuilder.DropTable(
                name: "Notification_User");

            migrationBuilder.DropTable(
                name: "Playlist_Track");

            migrationBuilder.DropTable(
                name: "Priority_Provider");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RunningTasks");

            migrationBuilder.DropTable(
                name: "Similar");

            migrationBuilder.DropTable(
                name: "SpecialItems");

            migrationBuilder.DropTable(
                name: "Specials");

            migrationBuilder.DropTable(
                name: "Track_User");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "VideoFiles");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "MusicGenres");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "EncoderProfiles");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Tvs");

            migrationBuilder.DropTable(
                name: "Libraries");
        }
    }
}
