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
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Browser = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Os = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Device = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CustomName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Version = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Ip = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EncoderProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Container = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Param = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncoderProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Task = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EpisodeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    CreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
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
                    Id = table.Column<string>(type: "TEXT", nullable: false),
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
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaStreams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStreams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "RunningTasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunningTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Logo = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
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
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Manage = table.Column<bool>(type: "INTEGER", nullable: false),
                    Owner = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Allowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    AudioTranscoding = table.Column<bool>(type: "INTEGER", nullable: false),
                    VideoTranscoding = table.Column<bool>(type: "INTEGER", nullable: false),
                    NoTranscoding = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EncoderProfileFolder",
                columns: table => new
                {
                    EncoderProfileId = table.Column<string>(type: "TEXT", nullable: false),
                    FolderId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncoderProfileFolder", x => new { x.EncoderProfileId, x.FolderId });
                    table.ForeignKey(
                        name: "FK_EncoderProfileFolder_EncoderProfiles_EncoderProfileId",
                        column: x => x.EncoderProfileId,
                        principalTable: "EncoderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncoderProfileFolder_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Cover = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Tracks = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Cover = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Parts = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "FolderLibrary",
                columns: table => new
                {
                    FolderId = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderLibrary", x => new { x.FolderId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_FolderLibrary_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageLibrary",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageLibrary", x => new { x.LanguageId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_LanguageLibrary_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: true),
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
                    Revenue = table.Column<long>(type: "INTEGER", nullable: true),
                    Runtime = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Tagline = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Trailer = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Video = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    VoteAverage = table.Column<double>(type: "REAL", nullable: true),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
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
                    NumberOfEpisodes = table.Column<int>(type: "INTEGER", nullable: false),
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
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                name: "PriorityProvider",
                columns: table => new
                {
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ProviderId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityProvider", x => new { x.Priority, x.Country, x.ProviderId });
                    table.ForeignKey(
                        name: "FK_PriorityProvider_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTrack",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTrack", x => new { x.LibraryId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_LibraryTrack_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicGenreTrack",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicGenreTrack", x => new { x.GenreId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_MusicGenreTrack_MusicGenres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicGenreTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTrack",
                columns: table => new
                {
                    PlaylistId = table.Column<string>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTrack", x => new { x.PlaylistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
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
                    TranslationId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Translations_TranslationId",
                        column: x => x.TranslationId,
                        principalTable: "Translations",
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
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                name: "LibraryUser",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryUser", x => new { x.LibraryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LibraryUser_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicPlays",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicPlays", x => new { x.UserId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_MusicPlays_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicPlays_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUser",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUser", x => new { x.NotificationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_NotificationUser_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackUser",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackUser", x => new { x.TrackId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TrackUser_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumLibrary",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumLibrary", x => new { x.AlbumId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_AlbumLibrary_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumMusicGenre",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MusicGenreId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumMusicGenre", x => new { x.AlbumId, x.MusicGenreId });
                    table.ForeignKey(
                        name: "FK_AlbumMusicGenre_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumMusicGenre_MusicGenres_MusicGenreId",
                        column: x => x.MusicGenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumTrack",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumTrack", x => new { x.AlbumId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_AlbumTrack_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumUser",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumUser", x => new { x.AlbumId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AlbumUser_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumArtist",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumArtist", x => new { x.AlbumId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistLibrary",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistLibrary", x => new { x.ArtistId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_ArtistLibrary_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistMusicGenre",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MusicGenreId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistMusicGenre", x => new { x.ArtistId, x.MusicGenreId });
                    table.ForeignKey(
                        name: "FK_ArtistMusicGenre_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistMusicGenre_MusicGenres_MusicGenreId",
                        column: x => x.MusicGenreId,
                        principalTable: "MusicGenres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistTrack",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTrack", x => new { x.ArtistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_ArtistTrack_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistUser",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistUser", x => new { x.ArtistId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ArtistUser_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionLibrary",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionLibrary", x => new { x.CollectionId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_CollectionLibrary_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionTranslation",
                columns: table => new
                {
                    CollectionsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionTranslation", x => new { x.CollectionsId, x.TranslationsId });
                    table.ForeignKey(
                        name: "FK_CollectionTranslation_Collections_CollectionsId",
                        column: x => x.CollectionsId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionTranslation_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificationMovie",
                columns: table => new
                {
                    CertificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificationMovie", x => new { x.CertificationId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_CertificationMovie_Certifications_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "Certifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificationMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionMovie",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionMovie", x => new { x.CollectionId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_CollectionMovie_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovie",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovie", x => new { x.GenreId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_GenreMovie_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordMovie",
                columns: table => new
                {
                    KeywordId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordMovie", x => new { x.KeywordId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_KeywordMovie_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryMovie",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryMovie", x => new { x.LibraryId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_LibraryMovie_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieTranslation",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTranslation", x => new { x.MoviesId, x.TranslationsId });
                    table.ForeignKey(
                        name: "FK_MovieTranslation_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTranslation_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlternativeTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iso31661 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
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
                name: "CertificationTv",
                columns: table => new
                {
                    CertificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificationTv", x => new { x.CertificationId, x.TvId });
                    table.ForeignKey(
                        name: "FK_CertificationTv_Certifications_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "Certifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificationTv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreTv",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreTv", x => new { x.GenreId, x.TvId });
                    table.ForeignKey(
                        name: "FK_GenreTv_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreTv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordTv",
                columns: table => new
                {
                    KeywordId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordTv", x => new { x.KeywordId, x.TvId });
                    table.ForeignKey(
                        name: "FK_KeywordTv_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordTv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTv",
                columns: table => new
                {
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    TvId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTv", x => new { x.LibraryId, x.TvId });
                    table.ForeignKey(
                        name: "FK_LibraryTv_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryTv_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    MediaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvFromId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvToId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieFromId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieToId = table.Column<int>(type: "INTEGER", nullable: true),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommendations_Movies_MovieFromId",
                        column: x => x.MovieFromId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Movies_MovieToId",
                        column: x => x.MovieToId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Tvs_TvFromId",
                        column: x => x.TvFromId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recommendations_Tvs_TvToId",
                        column: x => x.TvToId,
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Backdrop = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Overview = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TitleSort = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    MediaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvFromId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvToId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieFromId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieToId = table.Column<int>(type: "INTEGER", nullable: true),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Similar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Similar_Movies_MovieFromId",
                        column: x => x.MovieFromId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Movies_MovieToId",
                        column: x => x.MovieToId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Tvs_TvFromId",
                        column: x => x.TvFromId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Similar_Tvs_TvToId",
                        column: x => x.TvToId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranslationTv",
                columns: table => new
                {
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TvsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationTv", x => new { x.TranslationsId, x.TvsId });
                    table.ForeignKey(
                        name: "FK_TranslationTv_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TranslationTv_Tvs_TvsId",
                        column: x => x.TvsId,
                        principalTable: "Tvs",
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
                name: "SeasonTranslation",
                columns: table => new
                {
                    SeasonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTranslation", x => new { x.SeasonsId, x.TranslationsId });
                    table.ForeignKey(
                        name: "FK_SeasonTranslation_Seasons_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTranslation_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    JobId = table.Column<int>(type: "INTEGER", nullable: true)
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
                        name: "FK_Crews_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crews_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Crews_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
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
                name: "EpisodeTranslation",
                columns: table => new
                {
                    EpisodesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TranslationsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeTranslation", x => new { x.EpisodesId, x.TranslationsId });
                    table.ForeignKey(
                        name: "FK_EpisodeTranslation_Episodes_EpisodesId",
                        column: x => x.EpisodesId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeTranslation_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false)
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
                        name: "FK_GuestStars_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
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
                name: "VideoFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Filename = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Folder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    HostFolder = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Languages = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Quality = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Share = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Subtitles = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Chapters = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    table.ForeignKey(
                        name: "FK_VideoFiles_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileLibrary",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLibrary", x => new { x.FileId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_FileLibrary_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileLibrary_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileMovie",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMovie", x => new { x.FileId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_FileMovie_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileMovie_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Character = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EpisodeCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    GuestStarId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_GuestStars_GuestStarId",
                        column: x => x.GuestStarId,
                        principalTable: "GuestStars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Iso6391 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Site = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Src = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    VideoFileId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
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
                        name: "FK_Medias_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
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
                    table.ForeignKey(
                        name: "FK_Medias_VideoFiles_VideoFileId",
                        column: x => x.VideoFileId,
                        principalTable: "VideoFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    Played = table.Column<bool>(type: "INTEGER", nullable: true),
                    PlayCount = table.Column<int>(type: "INTEGER", nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: true),
                    PlaybackPositionTicks = table.Column<int>(type: "INTEGER", nullable: true),
                    LastPlayedDate = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Audio = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Subtitle = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    SubtitleType = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Time = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SpecialId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    VideoFileId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                        name: "FK_UserData_Specials_SpecialId",
                        column: x => x.SpecialId,
                        principalTable: "Specials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserData_Tvs_TvId",
                        column: x => x.TvId,
                        principalTable: "Tvs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserData_VideoFiles_VideoFileId",
                        column: x => x.VideoFileId,
                        principalTable: "VideoFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: true)
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
                        name: "FK_Casts_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Casts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
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
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    AspectRatio = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    Iso6391 = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Site = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Size = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    VoteAverage = table.Column<double>(type: "REAL", nullable: true),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: true),
                    CastCreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CastId = table.Column<int>(type: "INTEGER", nullable: true),
                    CrewCreditId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CrewId = table.Column<int>(type: "INTEGER", nullable: true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    ArtistId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AlbumId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TrackId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TvId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: true),
                    EpisodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true),
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ColorPalette = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Casts_CastId",
                        column: x => x.CastId,
                        principalTable: "Casts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Crews_CrewId",
                        column: x => x.CrewId,
                        principalTable: "Crews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Images_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Tvs_TvId",
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
                name: "IX_AlbumArtist_ArtistId",
                table: "AlbumArtist",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumLibrary_LibraryId",
                table: "AlbumLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMusicGenre_MusicGenreId",
                table: "AlbumMusicGenre",
                column: "MusicGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_LibraryId",
                table: "Albums",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTrack_TrackId",
                table: "AlbumTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumUser_UserId",
                table: "AlbumUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_MovieId",
                table: "AlternativeTitles",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_Title_MovieId",
                table: "AlternativeTitles",
                columns: new[] { "Title", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_Title_TvId",
                table: "AlternativeTitles",
                columns: new[] { "Title", "TvId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeTitles_TvId",
                table: "AlternativeTitles",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistLibrary_LibraryId",
                table: "ArtistLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMusicGenre_MusicGenreId",
                table: "ArtistMusicGenre",
                column: "MusicGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_LibraryId",
                table: "Artists",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_TrackId",
                table: "ArtistTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistUser_UserId",
                table: "ArtistUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_CreditId_EpisodeId_RoleId",
                table: "Casts",
                columns: new[] { "CreditId", "EpisodeId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_CreditId_MovieId_RoleId",
                table: "Casts",
                columns: new[] { "CreditId", "MovieId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_CreditId_SeasonId_RoleId",
                table: "Casts",
                columns: new[] { "CreditId", "SeasonId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_CreditId_TvId_RoleId",
                table: "Casts",
                columns: new[] { "CreditId", "TvId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_EpisodeId",
                table: "Casts",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_MovieId",
                table: "Casts",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_PersonId",
                table: "Casts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_RoleId",
                table: "Casts",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Casts_SeasonId",
                table: "Casts",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_TvId",
                table: "Casts",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificationMovie_MovieId",
                table: "CertificationMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_Iso31661_Rating",
                table: "Certifications",
                columns: new[] { "Iso31661", "Rating" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificationTv_TvId",
                table: "CertificationTv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionLibrary_LibraryId",
                table: "CollectionLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionMovie_MovieId",
                table: "CollectionMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_LibraryId",
                table: "Collections",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionTranslation_TranslationsId",
                table: "CollectionTranslation",
                column: "TranslationsId");

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
                name: "IX_Crews_CreditId_EpisodeId_JobId",
                table: "Crews",
                columns: new[] { "CreditId", "EpisodeId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_CreditId_MovieId_JobId",
                table: "Crews",
                columns: new[] { "CreditId", "MovieId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_CreditId_SeasonId_JobId",
                table: "Crews",
                columns: new[] { "CreditId", "SeasonId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_CreditId_TvId_JobId",
                table: "Crews",
                columns: new[] { "CreditId", "TvId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_EpisodeId",
                table: "Crews",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_JobId",
                table: "Crews",
                column: "JobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crews_MovieId",
                table: "Crews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_PersonId",
                table: "Crews",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_SeasonId",
                table: "Crews",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_TvId",
                table: "Crews",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_EncoderProfileFolder_FolderId",
                table: "EncoderProfileFolder",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeasonId",
                table: "Episodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_TvId",
                table: "Episodes",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeTranslation_TranslationsId",
                table: "EpisodeTranslation",
                column: "TranslationsId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLibrary_LibraryId",
                table: "FileLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMovie_MovieId",
                table: "FileMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_EpisodeId",
                table: "Files",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderLibrary_LibraryId",
                table: "FolderLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_Path",
                table: "Folders",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MovieId",
                table: "GenreMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreTv_TvId",
                table: "GenreTv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_CreditId_EpisodeId",
                table: "GuestStars",
                columns: new[] { "CreditId", "EpisodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_EpisodeId",
                table: "GuestStars",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_PersonId",
                table: "GuestStars",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AlbumId",
                table: "Images",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ArtistId",
                table: "Images",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CastId",
                table: "Images",
                column: "CastId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CollectionId",
                table: "Images",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CrewId",
                table: "Images",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_EpisodeId",
                table: "Images",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_AlbumId",
                table: "Images",
                columns: new[] { "FilePath", "AlbumId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_ArtistId",
                table: "Images",
                columns: new[] { "FilePath", "ArtistId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_CastCreditId",
                table: "Images",
                columns: new[] { "FilePath", "CastCreditId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_CollectionId",
                table: "Images",
                columns: new[] { "FilePath", "CollectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_CrewCreditId",
                table: "Images",
                columns: new[] { "FilePath", "CrewCreditId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_EpisodeId",
                table: "Images",
                columns: new[] { "FilePath", "EpisodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_MovieId",
                table: "Images",
                columns: new[] { "FilePath", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_PersonId",
                table: "Images",
                columns: new[] { "FilePath", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_SeasonId",
                table: "Images",
                columns: new[] { "FilePath", "SeasonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_TrackId",
                table: "Images",
                columns: new[] { "FilePath", "TrackId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath_TvId",
                table: "Images",
                columns: new[] { "FilePath", "TvId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_MovieId",
                table: "Images",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PersonId",
                table: "Images",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SeasonId",
                table: "Images",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TrackId",
                table: "Images",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TvId",
                table: "Images",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CreditId",
                table: "Jobs",
                column: "CreditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeywordMovie_MovieId",
                table: "KeywordMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordTv_TvId",
                table: "KeywordTv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLibrary_LibraryId",
                table: "LanguageLibrary",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Iso6391",
                table: "Languages",
                column: "Iso6391",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_Id",
                table: "Libraries",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMovie_MovieId",
                table: "LibraryMovie",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTrack_TrackId",
                table: "LibraryTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTv_TvId",
                table: "LibraryTv",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryUser_UserId",
                table: "LibraryUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_EpisodeId_Src",
                table: "Media",
                columns: new[] { "EpisodeId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_MovieId_Src",
                table: "Media",
                columns: new[] { "MovieId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_PersonId_Src",
                table: "Media",
                columns: new[] { "PersonId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_SeasonId_Src",
                table: "Media",
                columns: new[] { "SeasonId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_TvId_Src",
                table: "Media",
                columns: new[] { "TvId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_VideoFileId_Src",
                table: "Media",
                columns: new[] { "VideoFileId", "Src" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_LibraryId",
                table: "Movies",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieTranslation_TranslationsId",
                table: "MovieTranslation",
                column: "TranslationsId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenreTrack_TrackId",
                table: "MusicGenreTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicPlays_TrackId",
                table: "MusicPlays",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUser_UserId",
                table: "NotificationUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_People_TranslationId",
                table: "People",
                column: "TranslationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_TrackId",
                table: "PlaylistTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityProvider_ProviderId",
                table: "PriorityProvider",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MediaId_MovieFromId",
                table: "Recommendations",
                columns: new[] { "MediaId", "MovieFromId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MediaId_TvFromId",
                table: "Recommendations",
                columns: new[] { "MediaId", "TvFromId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MovieFromId",
                table: "Recommendations",
                column: "MovieFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MovieToId",
                table: "Recommendations",
                column: "MovieToId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TvFromId",
                table: "Recommendations",
                column: "TvFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TvToId",
                table: "Recommendations",
                column: "TvToId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreditId",
                table: "Roles",
                column: "CreditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuestStarId",
                table: "Roles",
                column: "GuestStarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_MovieId",
                table: "Seasons",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_TvId",
                table: "Seasons",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTranslation_TranslationsId",
                table: "SeasonTranslation",
                column: "TranslationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MediaId_MovieFromId",
                table: "Similar",
                columns: new[] { "MediaId", "MovieFromId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MediaId_TvFromId",
                table: "Similar",
                columns: new[] { "MediaId", "TvFromId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MovieFromId",
                table: "Similar",
                column: "MovieFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_MovieToId",
                table: "Similar",
                column: "MovieToId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_TvFromId",
                table: "Similar",
                column: "TvFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Similar_TvToId",
                table: "Similar",
                column: "TvToId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialItems_EpisodeId",
                table: "SpecialItems",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUser_UserId",
                table: "TrackUser",
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
                name: "IX_TranslationTv_TvsId",
                table: "TranslationTv",
                column: "TvsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tvs_LibraryId",
                table: "Tvs",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_MovieId_VideoFileId_UserId",
                table: "UserData",
                columns: new[] { "MovieId", "VideoFileId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserData_SpecialId",
                table: "UserData",
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_TvId",
                table: "UserData",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_UserId",
                table: "UserData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserData_VideoFileId",
                table: "UserData",
                column: "VideoFileId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoFiles_EpisodeId",
                table: "VideoFiles",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoFiles_Filename",
                table: "VideoFiles",
                column: "Filename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoFiles_MovieId",
                table: "VideoFiles",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "AlbumArtist");

            migrationBuilder.DropTable(
                name: "AlbumLibrary");

            migrationBuilder.DropTable(
                name: "AlbumMusicGenre");

            migrationBuilder.DropTable(
                name: "AlbumTrack");

            migrationBuilder.DropTable(
                name: "AlbumUser");

            migrationBuilder.DropTable(
                name: "AlternativeTitles");

            migrationBuilder.DropTable(
                name: "ArtistLibrary");

            migrationBuilder.DropTable(
                name: "ArtistMusicGenre");

            migrationBuilder.DropTable(
                name: "ArtistTrack");

            migrationBuilder.DropTable(
                name: "ArtistUser");

            migrationBuilder.DropTable(
                name: "CertificationMovie");

            migrationBuilder.DropTable(
                name: "CertificationTv");

            migrationBuilder.DropTable(
                name: "CollectionLibrary");

            migrationBuilder.DropTable(
                name: "CollectionMovie");

            migrationBuilder.DropTable(
                name: "CollectionTranslation");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Creators");

            migrationBuilder.DropTable(
                name: "EncoderProfileFolder");

            migrationBuilder.DropTable(
                name: "EpisodeTranslation");

            migrationBuilder.DropTable(
                name: "FileLibrary");

            migrationBuilder.DropTable(
                name: "FileMovie");

            migrationBuilder.DropTable(
                name: "FolderLibrary");

            migrationBuilder.DropTable(
                name: "GenreMovie");

            migrationBuilder.DropTable(
                name: "GenreTv");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "KeywordMovie");

            migrationBuilder.DropTable(
                name: "KeywordTv");

            migrationBuilder.DropTable(
                name: "LanguageLibrary");

            migrationBuilder.DropTable(
                name: "LibraryMovie");

            migrationBuilder.DropTable(
                name: "LibraryTrack");

            migrationBuilder.DropTable(
                name: "LibraryTv");

            migrationBuilder.DropTable(
                name: "LibraryUser");

            migrationBuilder.DropTable(
                name: "MediaAttachments");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "MediaStreams");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "MovieTranslation");

            migrationBuilder.DropTable(
                name: "MusicGenreTrack");

            migrationBuilder.DropTable(
                name: "MusicPlays");

            migrationBuilder.DropTable(
                name: "NotificationUser");

            migrationBuilder.DropTable(
                name: "PlaylistTrack");

            migrationBuilder.DropTable(
                name: "PriorityProvider");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "RunningTasks");

            migrationBuilder.DropTable(
                name: "SeasonTranslation");

            migrationBuilder.DropTable(
                name: "Similar");

            migrationBuilder.DropTable(
                name: "SpecialItems");

            migrationBuilder.DropTable(
                name: "TrackUser");

            migrationBuilder.DropTable(
                name: "TranslationTv");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "EncoderProfiles");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "MusicGenres");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Specials");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VideoFiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "GuestStars");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Tvs");

            migrationBuilder.DropTable(
                name: "Libraries");
        }
    }
}
