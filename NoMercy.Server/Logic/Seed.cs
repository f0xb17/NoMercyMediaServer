using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.MusicBrainz.Client;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Certifications;
using NoMercy.Server.app.Helper;
using File = System.IO.File;
using Genre = NoMercy.Database.Models.Genre;
using LogLevel = NoMercy.Helpers.LogLevel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.Logic;

public static class Seed
{
    private static ConfigClient ConfigClient { get; set; } = new();
    private static MovieClient MovieClient { get; set; } = new();
    private static TvClient TvClient { get; set; } = new();
    private static readonly MediaContext MediaContext = new();
    private static Folder[] _folders = [];
    private static User[] _users = [];

    public static async Task Init()
    {
        await CreateDatabase();
        await SeedDatabase();
    }

    private static async Task CreateDatabase()
    {
        await MediaContext.Database.EnsureCreatedAsync();
        await MediaContext.SaveChangesAsync();

        await Databases.QueueContext.Database.EnsureCreatedAsync();
        await Databases.QueueContext.SaveChangesAsync();
    }

    private static async Task SeedDatabase()
    {
        try
        {
            await AddGenres();
            await AddCertifications();
            await AddLanguages();
            await AddCountries();
            await AddMusicGenres();
            await AddFolderRoots();
            await AddEncoderProfiles();
            await AddLibraries();
            await GetUsers();
            // await AddSpecial();
        }
        catch (Exception e)
        {
            Logger.Setup(e, LogLevel.Error);
            throw;
        }
    }

    private static async Task AddEncoderProfiles()
    {
        EncoderProfileDto[] encoderProfiles;
        if (File.Exists(AppFiles.EncoderProfilesSeedFile))
        {
            encoderProfiles = JsonConvert.DeserializeObject<EncoderProfileDto[]>(
                await File.ReadAllTextAsync(AppFiles.EncoderProfilesSeedFile)) ?? [];
        }
        else
        {
            encoderProfiles =
            [
                new EncoderProfileDto
                {
                    Id = Ulid.Parse("01HQ6298ZSZYKJT83WDWTPG4G8"),
                    Name = "2160p high",
                    Container = "auto",
                    Params = new EncoderProfileParamsDto
                    {
                        Width = 3840,
                        Crf = 20,
                        Preset = "slow",
                        Profile = "high",
                        Codec = "H.264",
                        Audio = "libfdk_aac"
                    }
                },
                new EncoderProfileDto
                {
                    Id = Ulid.Parse("01HQ629JAYQDEQAH0GW3ZHGW8Z"),
                    Name = "1080p high",
                    Container = "auto",
                    Params = new EncoderProfileParamsDto
                    {
                        Width = 1920,
                        Crf = 20,
                        Preset = "slow",
                        Profile = "high",
                        Codec = "H.264",
                        Audio = "libfdk_aac"
                    }
                },
                new EncoderProfileDto
                {
                    Id = Ulid.Parse("01HQ629SJ32FTV2Q46NX3H1CK9"),
                    Name = "1080p regular",
                    Container = "auto",
                    Params = new EncoderProfileParamsDto
                    {
                        Width = 1920,
                        Crf = 25,
                        Preset = "slow",
                        Profile = "high",
                        Codec = "H.264",
                        Audio = "libfdk_aac"
                    }
                },
                new EncoderProfileDto
                {
                    Id = Ulid.Parse("01HR360AKTW47XC6ZQ2V9DF024"),
                    Name = "1080p low",
                    Container = "auto",
                    Params = new EncoderProfileParamsDto
                    {
                        Width = 1920,
                        Crf = 28,
                        Preset = "slow",
                        Profile = "high",
                        Codec = "H.264",
                        Audio = "libfdk_aac"
                    }
                }
            ];
        }

        await MediaContext.EncoderProfiles.UpsertRange(encoderProfiles.ToList()
                .ConvertAll<EncoderProfile>(encoderProfile => new EncoderProfile
                {
                    Id = encoderProfile.Id,
                    Name = encoderProfile.Name,
                    Container = encoderProfile.Container,
                    Param = JsonConvert.SerializeObject(new EncoderProfileParamsDto
                    {
                        Width = encoderProfile.Params.Width,
                        Crf = encoderProfile.Params.Crf,
                        Preset = encoderProfile.Params.Preset,
                        Profile = encoderProfile.Params.Profile,
                        Codec = encoderProfile.Params.Codec,
                        Audio = encoderProfile.Params.Audio,
                    }),
                })
            )
            .On(v => new { v.Id })
            .WhenMatched((vs, vi) => new EncoderProfile
            {
                Id = vi.Id,
                Name = vi.Name,
                Container = vi.Container,
                Param = vi.Param,
            })
            .RunAsync();
    }

    private static async Task GetUsers()
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "NoMercy");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

        IDictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("server_id", SystemInfo.DeviceId.ToString());

        string newUrl = QueryHelpers.AddQueryString("https://api-dev.nomercy.tv/v1/server/users", query);

        HttpResponseMessage? response = await client.GetAsync(newUrl);
        string? content = await response.Content.ReadAsStringAsync();

        if (content == null) throw new Exception("Failed to get Server info");

        ServerUserDto[] serverUsers = JsonConvert.DeserializeObject<ServerUserDto[]>(content) ?? [];

        _users = serverUsers.ToList()
            .ConvertAll<User>(serverUser => new User
            {
                Id = serverUser.UserId,
                Email = serverUser.Email,
                Name = serverUser.Name,
                Allowed = serverUser.Enabled,
                Manage = serverUser.Enabled,
                AudioTranscoding = serverUser.Enabled,
                NoTranscoding = serverUser.Enabled,
                VideoTranscoding = serverUser.Enabled,
                Owner = serverUser.IsOwner,
                UpdatedAt = DateTime.Now,
            })
            .ToArray();

        await MediaContext.Users
            .UpsertRange(_users)
            .On(v => new { v.Id })
            .WhenMatched((us, ui) => new User
            {
                Id = ui.Id,
                Email = ui.Email,
                Name = ui.Name,
                Allowed = ui.Allowed,
                Manage = ui.Manage,
                AudioTranscoding = ui.AudioTranscoding,
                NoTranscoding = ui.NoTranscoding,
                VideoTranscoding = ui.VideoTranscoding,
                Owner = ui.Owner,
                UpdatedAt = ui.UpdatedAt,
            })
            .RunAsync();

        if (!File.Exists(AppFiles.LibrariesSeedFile)) return;

        Library[] libraries =
            JsonConvert.DeserializeObject<Library[]>(
                await File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? [];

        List<LibraryUser> libraryUsers = [];

        foreach (User user in _users.ToList())
        {
            foreach (Library library in libraries.ToList())
            {
                libraryUsers.Add(new LibraryUser
                {
                    LibraryId = library.Id,
                    UserId = user.Id,
                });
            }
        }

        await MediaContext.LibraryUser
            .UpsertRange(libraryUsers)
            .On(v => new { v.LibraryId, v.UserId })
            .WhenMatched((lus, lui) => new LibraryUser
            {
                LibraryId = lui.LibraryId,
                UserId = lui.UserId,
            })
            .RunAsync();
    }

    private static async Task AddGenres()
    {
        List<Genre> genres = [];

        genres.AddRange(
            (await MovieClient.Genres())?
            .Genres.ToList()
            .ConvertAll<Genre>(x => new Genre(x)).ToArray() ?? []
        );

        genres.AddRange(
            (await TvClient.Genres())?
            .Genres.ToList()
            .ConvertAll<Genre>(x => new Genre(x)).ToArray() ?? []
        );

        await MediaContext.Genres.UpsertRange(genres)
            .On(v => new { v.Id })
            .WhenMatched(v => new Genre
            {
                Id = v.Id,
                Name = v.Name
            })
            .RunAsync();
    }

    private static async Task AddCertifications()
    {
        List<Certification> certifications = [];

        foreach (KeyValuePair<string?, MovieCertification[]> keyValuePair in (await MovieClient.Certifications())
                 ?.Certifications ?? [])
        {
            foreach (MovieCertification certification in keyValuePair.Value)
            {
                certifications.Add(new Certification(keyValuePair.Key, certification));
            }
        }

        foreach (KeyValuePair<string?, TvShowCertification[]> keyValuePair in (await TvClient.Certifications())
                 ?.Certifications ?? [])
        {
            foreach (TvShowCertification certification in keyValuePair.Value)
            {
                certifications.Add(new Certification(keyValuePair.Key, certification));
            }
        }

        await MediaContext.Certifications.UpsertRange(certifications)
            .On(v => new { v.Iso31661, v.Rating })
            .WhenMatched(v => new Certification
            {
                Iso31661 = v.Iso31661,
                Rating = v.Rating,
                Meaning = v.Meaning,
                Order = v.Order
            })
            .RunAsync();
    }

    private static async Task AddLanguages()
    {
        Language[] languages = (await ConfigClient.Languages())?.ToList()
            .ConvertAll<Language>(language => new Language(language)).ToArray() ?? [];

        await MediaContext.Languages.UpsertRange(languages)
            .On(v => new { v.Iso6391 })
            .WhenMatched(v => new Language
            {
                Iso6391 = v.Iso6391,
                Name = v.Name,
                EnglishName = v.EnglishName,
            })
            .RunAsync();
    }

    private static async Task AddCountries()
    {
        Country[] countries = (await ConfigClient.Countries())?.ToList()
            .ConvertAll<Country>(country => new Country(country)).ToArray() ?? [];

        await MediaContext.Countries.UpsertRange(countries)
            .On(v => new { v.Iso31661 })
            .WhenMatched(v => new Country
            {
                Iso31661 = v.Iso31661,
                NativeName = v.NativeName,
                EnglishName = v.EnglishName,
            })
            .RunAsync();
    }

    private static async Task AddMusicGenres()
    {
        GenreClient genreClient = new();

        MusicGenre[] genres = (await genreClient.All()).ToList()
            .ConvertAll<MusicGenre>(genre => new MusicGenre(genre)).ToArray();

        await MediaContext.MusicGenres.UpsertRange(genres)
            .On(v => new { v.Id })
            .WhenMatched(v => new MusicGenre
            {
                Id = v.Id,
                Name = v.Name,
            })
            .RunAsync();

        await Task.CompletedTask;
    }

    private static async Task AddFolderRoots()
    {
        try
        {
            if (!File.Exists(AppFiles.FolderRootsSeedFile)) return;

            _folders = JsonConvert.DeserializeObject<Folder[]>(
                await File.ReadAllTextAsync(AppFiles.FolderRootsSeedFile)) ?? [];

            await MediaContext.Folders.UpsertRange(_folders)
                .On(v => new { v.Path })
                .WhenMatched((vs, vi) => new Folder()
                {
                    Id = vi.Id,
                    Path = vi.Path,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e, LogLevel.Error);
        }
    }

    private static async Task AddLibraries()
    {
        try
        {
            if (!File.Exists(AppFiles.LibrariesSeedFile)) return;

            List<LibrarySeedDto> librarySeed =
                (JsonConvert.DeserializeObject<LibrarySeedDto[]>(
                    await File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? Array.Empty<LibrarySeedDto>())
                .ToList();

            List<Library> libraries = librarySeed.Select(librarySeedDto => new Library()
            {
                Id = librarySeedDto.Id,
                AutoRefreshInterval = librarySeedDto.AutoRefreshInterval,
                ChapterImages = librarySeedDto.ChapterImages,
                ExtractChapters = librarySeedDto.ExtractChapters,
                ExtractChaptersDuring = librarySeedDto.ExtractChaptersDuring,
                Image = librarySeedDto.Image,
                PerfectSubtitleMatch = librarySeedDto.PerfectSubtitleMatch,
                Realtime = librarySeedDto.Realtime,
                SpecialSeasonName = librarySeedDto.SpecialSeasonName,
                Title = librarySeedDto.Title,
                Type = librarySeedDto.Type,
                Order = librarySeedDto.Order,
            }).ToList();

            await MediaContext.Libraries.UpsertRange(libraries)
                .On(v => new { v.Id })
                .WhenMatched((vs, vi) => new Library()
                {
                    Id = vi.Id,
                    AutoRefreshInterval = vi.AutoRefreshInterval,
                    ChapterImages = vi.ChapterImages,
                    ExtractChapters = vi.ExtractChapters,
                    ExtractChaptersDuring = vi.ExtractChaptersDuring,
                    Image = vi.Image,
                    PerfectSubtitleMatch = vi.PerfectSubtitleMatch,
                    Realtime = vi.Realtime,
                    SpecialSeasonName = vi.SpecialSeasonName,
                    Title = vi.Title,
                    Type = vi.Type,
                    Order = vi.Order,
                })
                .RunAsync();

            List<FolderLibrary> libraryFolders = [];

            foreach (LibrarySeedDto library in librarySeed.ToList())
            {
                foreach (FolderDto folder in library.Folders.ToList())
                {
                    libraryFolders.Add(new FolderLibrary(folder.Id, library.Id));
                }
            }

            await MediaContext.FolderLibrary
                .UpsertRange(libraryFolders)
                .On(v => new { v.FolderId, v.LibraryId })
                .WhenMatched((vs, vi) => new FolderLibrary()
                {
                    FolderId = vi.FolderId,
                    LibraryId = vi.LibraryId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.Setup(e, LogLevel.Error);
        }
    }

    private static async Task AddSpecial()
    {
        List<SpecialItemDto> specialData =
        [
            new SpecialItemDto { Type = "movie", Id = 1771 },
            new SpecialItemDto { Type = "episode", Id = 1013214 },
            new SpecialItemDto { Type = "episode", Id = 1026747 },
            new SpecialItemDto { Type = "episode", Id = 1026748 },
            new SpecialItemDto { Type = "episode", Id = 1034250 },
            new SpecialItemDto { Type = "episode", Id = 1037986 },
            new SpecialItemDto { Type = "episode", Id = 1037987 },
            new SpecialItemDto { Type = "episode", Id = 1037988 },
            new SpecialItemDto { Type = "episode", Id = 1037989 },
            new SpecialItemDto { Type = "episode", Id = 1137077 },
            new SpecialItemDto { Type = "episode", Id = 1143050 },
            new SpecialItemDto { Type = "episode", Id = 1148390 },
            new SpecialItemDto { Type = "episode", Id = 1159990 },
            new SpecialItemDto { Type = "episode", Id = 1160670 },
            new SpecialItemDto { Type = "episode", Id = 1165223 },
            new SpecialItemDto { Type = "episode", Id = 1165224 },
            new SpecialItemDto { Type = "episode", Id = 1165523 },
            new SpecialItemDto { Type = "episode", Id = 1165524 },
            new SpecialItemDto { Type = "episode", Id = 1165525 },
            new SpecialItemDto { Type = "movie", Id = 211387 },
            new SpecialItemDto { Type = "movie", Id = 299537 },
            new SpecialItemDto { Type = "movie", Id = 1726 },
            new SpecialItemDto { Type = "movie", Id = 10138 },
            new SpecialItemDto { Type = "movie", Id = 1724 },
            new SpecialItemDto { Type = "movie", Id = 10195 },
            new SpecialItemDto { Type = "movie", Id = 76122 },
            new SpecialItemDto { Type = "movie", Id = 76535 },
            new SpecialItemDto { Type = "movie", Id = 448366 },
            new SpecialItemDto { Type = "movie", Id = 76338 },
            new SpecialItemDto { Type = "movie", Id = 68721 },
            new SpecialItemDto { Type = "movie", Id = 100402 },
            new SpecialItemDto { Type = "episode", Id = 972992 },
            new SpecialItemDto { Type = "episode", Id = 63412 },
            new SpecialItemDto { Type = "episode", Id = 63413 },
            new SpecialItemDto { Type = "episode", Id = 63414 },
            new SpecialItemDto { Type = "episode", Id = 63415 },
            new SpecialItemDto { Type = "episode", Id = 63416 },
            new SpecialItemDto { Type = "episode", Id = 63417 },
            new SpecialItemDto { Type = "episode", Id = 63418 },
            new SpecialItemDto { Type = "episode", Id = 63419 },
            new SpecialItemDto { Type = "episode", Id = 63420 },
            new SpecialItemDto { Type = "episode", Id = 63421 },
            new SpecialItemDto { Type = "episode", Id = 63422 },
            new SpecialItemDto { Type = "episode", Id = 63423 },
            new SpecialItemDto { Type = "episode", Id = 971804 },
            new SpecialItemDto { Type = "episode", Id = 971979 },
            new SpecialItemDto { Type = "episode", Id = 973093 },
            new SpecialItemDto { Type = "episode", Id = 973397 },
            new SpecialItemDto { Type = "episode", Id = 974019 },
            new SpecialItemDto { Type = "episode", Id = 974709 },
            new SpecialItemDto { Type = "episode", Id = 974655 },
            new SpecialItemDto { Type = "episode", Id = 975266 },
            new SpecialItemDto { Type = "episode", Id = 975473 },
            new SpecialItemDto { Type = "movie", Id = 118340 },
            new SpecialItemDto { Type = "movie", Id = 283995 },
            new SpecialItemDto { Type = "episode", Id = 1036348 },
            new SpecialItemDto { Type = "episode", Id = 1036350 },
            new SpecialItemDto { Type = "episode", Id = 1036351 },
            new SpecialItemDto { Type = "episode", Id = 1036352 },
            new SpecialItemDto { Type = "episode", Id = 1036353 },
            new SpecialItemDto { Type = "episode", Id = 1036354 },
            new SpecialItemDto { Type = "episode", Id = 1036355 },
            new SpecialItemDto { Type = "episode", Id = 1036356 },
            new SpecialItemDto { Type = "episode", Id = 1036357 },
            new SpecialItemDto { Type = "episode", Id = 1036358 },
            new SpecialItemDto { Type = "episode", Id = 1036359 },
            new SpecialItemDto { Type = "episode", Id = 1036360 },
            new SpecialItemDto { Type = "episode", Id = 1036361 },
            new SpecialItemDto { Type = "movie", Id = 99861 },
            new SpecialItemDto { Type = "episode", Id = 1000647 },
            new SpecialItemDto { Type = "episode", Id = 1007159 },
            new SpecialItemDto { Type = "episode", Id = 1009280 },
            new SpecialItemDto { Type = "episode", Id = 1010174 },
            new SpecialItemDto { Type = "episode", Id = 1010683 },
            new SpecialItemDto { Type = "episode", Id = 1010684 },
            new SpecialItemDto { Type = "episode", Id = 1013262 },
            new SpecialItemDto { Type = "episode", Id = 1016544 },
            new SpecialItemDto { Type = "episode", Id = 1018666 },
            new SpecialItemDto { Type = "episode", Id = 1019800 },
            new SpecialItemDto { Type = "episode", Id = 1019801 },
            new SpecialItemDto { Type = "episode", Id = 1043555 },
            new SpecialItemDto { Type = "episode", Id = 1043556 },
            new SpecialItemDto { Type = "episode", Id = 1046566 },
            new SpecialItemDto { Type = "episode", Id = 1046817 },
            new SpecialItemDto { Type = "episode", Id = 1048738 },
            new SpecialItemDto { Type = "episode", Id = 1049931 },
            new SpecialItemDto { Type = "episode", Id = 1051253 },
            new SpecialItemDto { Type = "episode", Id = 1051254 },
            new SpecialItemDto { Type = "episode", Id = 1051255 },
            new SpecialItemDto { Type = "episode", Id = 1051256 },
            new SpecialItemDto { Type = "episode", Id = 1051257 },
            new SpecialItemDto { Type = "movie", Id = 102899 },
            new SpecialItemDto { Type = "episode", Id = 1105754 },
            new SpecialItemDto { Type = "episode", Id = 1105755 },
            new SpecialItemDto { Type = "episode", Id = 1105756 },
            new SpecialItemDto { Type = "episode", Id = 1105757 },
            new SpecialItemDto { Type = "episode", Id = 1105758 },
            new SpecialItemDto { Type = "episode", Id = 1105759 },
            new SpecialItemDto { Type = "episode", Id = 1105760 },
            new SpecialItemDto { Type = "episode", Id = 1105761 },
            new SpecialItemDto { Type = "episode", Id = 1105762 },
            new SpecialItemDto { Type = "episode", Id = 1105763 },
            new SpecialItemDto { Type = "episode", Id = 1105764 },
            new SpecialItemDto { Type = "episode", Id = 1105765 },
            new SpecialItemDto { Type = "episode", Id = 1105766 },
            new SpecialItemDto { Type = "episode", Id = 1167028 },
            new SpecialItemDto { Type = "episode", Id = 1175503 },
            new SpecialItemDto { Type = "episode", Id = 1175504 },
            new SpecialItemDto { Type = "episode", Id = 1175505 },
            new SpecialItemDto { Type = "episode", Id = 1175506 },
            new SpecialItemDto { Type = "episode", Id = 1175507 },
            new SpecialItemDto { Type = "episode", Id = 1175508 },
            new SpecialItemDto { Type = "episode", Id = 1175509 },
            new SpecialItemDto { Type = "episode", Id = 1175510 },
            new SpecialItemDto { Type = "episode", Id = 1175511 },
            new SpecialItemDto { Type = "episode", Id = 1175512 },
            new SpecialItemDto { Type = "episode", Id = 1175513 },
            new SpecialItemDto { Type = "episode", Id = 1175514 },
            new SpecialItemDto { Type = "movie", Id = 271110 },
            new SpecialItemDto { Type = "episode", Id = 1085277 },
            new SpecialItemDto { Type = "episode", Id = 1085278 },
            new SpecialItemDto { Type = "episode", Id = 1109828 },
            new SpecialItemDto { Type = "episode", Id = 1112975 },
            new SpecialItemDto { Type = "episode", Id = 1117287 },
            new SpecialItemDto { Type = "episode", Id = 1133154 },
            new SpecialItemDto { Type = "episode", Id = 1134492 },
            new SpecialItemDto { Type = "episode", Id = 1134703 },
            new SpecialItemDto { Type = "episode", Id = 1137415 },
            new SpecialItemDto { Type = "episode", Id = 1142067 },
            new SpecialItemDto { Type = "episode", Id = 1173978 },
            new SpecialItemDto { Type = "episode", Id = 1175551 },
            new SpecialItemDto { Type = "episode", Id = 1176885 },
            new SpecialItemDto { Type = "episode", Id = 1180188 },
            new SpecialItemDto { Type = "episode", Id = 1180189 },
            new SpecialItemDto { Type = "episode", Id = 1182846 },
            new SpecialItemDto { Type = "episode", Id = 1184330 },
            new SpecialItemDto { Type = "episode", Id = 1187281 },
            new SpecialItemDto { Type = "episode", Id = 1189229 },
            new SpecialItemDto { Type = "episode", Id = 1191831 },
            new SpecialItemDto { Type = "episode", Id = 1191832 },
            new SpecialItemDto { Type = "episode", Id = 1191833 },
            new SpecialItemDto { Type = "episode", Id = 1045315 },
            new SpecialItemDto { Type = "episode", Id = 1221122 },
            new SpecialItemDto { Type = "episode", Id = 1221123 },
            new SpecialItemDto { Type = "episode", Id = 1221124 },
            new SpecialItemDto { Type = "episode", Id = 1221125 },
            new SpecialItemDto { Type = "episode", Id = 1221126 },
            new SpecialItemDto { Type = "episode", Id = 1221127 },
            new SpecialItemDto { Type = "episode", Id = 1221128 },
            new SpecialItemDto { Type = "episode", Id = 1221129 },
            new SpecialItemDto { Type = "episode", Id = 1221130 },
            new SpecialItemDto { Type = "episode", Id = 1221131 },
            new SpecialItemDto { Type = "episode", Id = 1221132 },
            new SpecialItemDto { Type = "episode", Id = 1221133 },
            new SpecialItemDto { Type = "movie", Id = 497698 },
            new SpecialItemDto { Type = "movie", Id = 315635 },
            new SpecialItemDto { Type = "movie", Id = 284054 },
            new SpecialItemDto { Type = "movie", Id = 284052 },
            new SpecialItemDto { Type = "episode", Id = 1045314 },
            new SpecialItemDto { Type = "episode", Id = 1271969 },
            new SpecialItemDto { Type = "episode", Id = 1271970 },
            new SpecialItemDto { Type = "episode", Id = 1271971 },
            new SpecialItemDto { Type = "episode", Id = 1271972 },
            new SpecialItemDto { Type = "episode", Id = 1271973 },
            new SpecialItemDto { Type = "episode", Id = 1271974 },
            new SpecialItemDto { Type = "episode", Id = 1271975 },
            new SpecialItemDto { Type = "episode", Id = 1271976 },
            new SpecialItemDto { Type = "episode", Id = 1271977 },
            new SpecialItemDto { Type = "episode", Id = 1271978 },
            new SpecialItemDto { Type = "episode", Id = 1271979 },
            new SpecialItemDto { Type = "episode", Id = 1271980 },
            new SpecialItemDto { Type = "episode", Id = 1206223 },
            new SpecialItemDto { Type = "episode", Id = 1223126 },
            new SpecialItemDto { Type = "episode", Id = 1226945 },
            new SpecialItemDto { Type = "episode", Id = 1228589 },
            new SpecialItemDto { Type = "episode", Id = 1229423 },
            new SpecialItemDto { Type = "episode", Id = 1232364 },
            new SpecialItemDto { Type = "episode", Id = 1236671 },
            new SpecialItemDto { Type = "episode", Id = 1236672 },
            new SpecialItemDto { Type = "episode", Id = 1236673 },
            new SpecialItemDto { Type = "episode", Id = 1251661 },
            new SpecialItemDto { Type = "episode", Id = 1252726 },
            new SpecialItemDto { Type = "episode", Id = 1252728 },
            new SpecialItemDto { Type = "episode", Id = 1252729 },
            new SpecialItemDto { Type = "episode", Id = 1252731 },
            new SpecialItemDto { Type = "episode", Id = 1265156 },
            new SpecialItemDto { Type = "episode", Id = 1275764 },
            new SpecialItemDto { Type = "episode", Id = 1298641 },
            new SpecialItemDto { Type = "episode", Id = 1298642 },
            new SpecialItemDto { Type = "episode", Id = 1302044 },
            new SpecialItemDto { Type = "episode", Id = 1304542 },
            new SpecialItemDto { Type = "episode", Id = 1310650 },
            new SpecialItemDto { Type = "episode", Id = 1310651 },
            new SpecialItemDto { Type = "episode", Id = 1243265 },
            new SpecialItemDto { Type = "episode", Id = 1278477 },
            new SpecialItemDto { Type = "episode", Id = 1336814 },
            new SpecialItemDto { Type = "episode", Id = 1336815 },
            new SpecialItemDto { Type = "episode", Id = 1336816 },
            new SpecialItemDto { Type = "episode", Id = 1336817 },
            new SpecialItemDto { Type = "episode", Id = 1336818 },
            new SpecialItemDto { Type = "episode", Id = 1336819 },
            new SpecialItemDto { Type = "episode", Id = 1279700 },
            new SpecialItemDto { Type = "episode", Id = 1332116 },
            new SpecialItemDto { Type = "episode", Id = 1367789 },
            new SpecialItemDto { Type = "episode", Id = 1367945 },
            new SpecialItemDto { Type = "episode", Id = 1367946 },
            new SpecialItemDto { Type = "episode", Id = 1367947 },
            new SpecialItemDto { Type = "episode", Id = 1367948 },
            new SpecialItemDto { Type = "episode", Id = 1367949 },
            new SpecialItemDto { Type = "movie", Id = 284053 },
            new SpecialItemDto { Type = "episode", Id = 1209036 },
            new SpecialItemDto { Type = "episode", Id = 1209319 },
            new SpecialItemDto { Type = "episode", Id = 1209320 },
            new SpecialItemDto { Type = "episode", Id = 1209321 },
            new SpecialItemDto { Type = "episode", Id = 1209322 },
            new SpecialItemDto { Type = "episode", Id = 1209323 },
            new SpecialItemDto { Type = "episode", Id = 1209324 },
            new SpecialItemDto { Type = "episode", Id = 1209325 },
            new SpecialItemDto { Type = "episode", Id = 1209326 },
            new SpecialItemDto { Type = "episode", Id = 1209327 },
            new SpecialItemDto { Type = "episode", Id = 1209328 },
            new SpecialItemDto { Type = "episode", Id = 1209329 },
            new SpecialItemDto { Type = "episode", Id = 1209330 },
            new SpecialItemDto { Type = "episode", Id = 1403908 },
            new SpecialItemDto { Type = "episode", Id = 1437006 },
            new SpecialItemDto { Type = "episode", Id = 1437007 },
            new SpecialItemDto { Type = "episode", Id = 1437008 },
            new SpecialItemDto { Type = "episode", Id = 1437010 },
            new SpecialItemDto { Type = "episode", Id = 1437011 },
            new SpecialItemDto { Type = "episode", Id = 1437012 },
            new SpecialItemDto { Type = "episode", Id = 1437014 },
            new SpecialItemDto { Type = "episode", Id = 1437015 },
            new SpecialItemDto { Type = "episode", Id = 1437016 },
            new SpecialItemDto { Type = "episode", Id = 1437017 },
            new SpecialItemDto { Type = "episode", Id = 1437019 },
            new SpecialItemDto { Type = "episode", Id = 1437020 },
            new SpecialItemDto { Type = "episode", Id = 1455055 },
            new SpecialItemDto { Type = "episode", Id = 1455056 },
            new SpecialItemDto { Type = "episode", Id = 1455057 },
            new SpecialItemDto { Type = "episode", Id = 1455058 },
            new SpecialItemDto { Type = "episode", Id = 1455059 },
            new SpecialItemDto { Type = "episode", Id = 1455060 },
            new SpecialItemDto { Type = "episode", Id = 1455061 },
            new SpecialItemDto { Type = "episode", Id = 1455062 },
            new SpecialItemDto { Type = "episode", Id = 1455063 },
            new SpecialItemDto { Type = "episode", Id = 1455064 },
            new SpecialItemDto { Type = "episode", Id = 1455065 },
            new SpecialItemDto { Type = "episode", Id = 1455066 },
            new SpecialItemDto { Type = "episode", Id = 1455067 },
            new SpecialItemDto { Type = "episode", Id = 1419370 },
            new SpecialItemDto { Type = "episode", Id = 1480153 },
            new SpecialItemDto { Type = "episode", Id = 1497462 },
            new SpecialItemDto { Type = "episode", Id = 1497463 },
            new SpecialItemDto { Type = "episode", Id = 1497834 },
            new SpecialItemDto { Type = "episode", Id = 1508514 },
            new SpecialItemDto { Type = "episode", Id = 1515877 },
            new SpecialItemDto { Type = "episode", Id = 1515876 },
            new SpecialItemDto { Type = "episode", Id = 1518261 },
            new SpecialItemDto { Type = "episode", Id = 1526687 },
            new SpecialItemDto { Type = "episode", Id = 1215640 },
            new SpecialItemDto { Type = "episode", Id = 1378905 },
            new SpecialItemDto { Type = "episode", Id = 1390713 },
            new SpecialItemDto { Type = "episode", Id = 1393196 },
            new SpecialItemDto { Type = "episode", Id = 1399377 },
            new SpecialItemDto { Type = "episode", Id = 1399378 },
            new SpecialItemDto { Type = "episode", Id = 1399379 },
            new SpecialItemDto { Type = "episode", Id = 1404444 },
            new SpecialItemDto { Type = "episode", Id = 1404445 },
            new SpecialItemDto { Type = "episode", Id = 1404446 },
            new SpecialItemDto { Type = "episode", Id = 1575105 },
            new SpecialItemDto { Type = "episode", Id = 1597865 },
            new SpecialItemDto { Type = "episode", Id = 1597866 },
            new SpecialItemDto { Type = "episode", Id = 1597867 },
            new SpecialItemDto { Type = "episode", Id = 1597868 },
            new SpecialItemDto { Type = "episode", Id = 1597869 },
            new SpecialItemDto { Type = "episode", Id = 1597870 },
            new SpecialItemDto { Type = "episode", Id = 1597871 },
            new SpecialItemDto { Type = "episode", Id = 1597872 },
            new SpecialItemDto { Type = "episode", Id = 1597873 },
            new SpecialItemDto { Type = "episode", Id = 1597874 },
            new SpecialItemDto { Type = "episode", Id = 1597875 },
            new SpecialItemDto { Type = "episode", Id = 1597876 },
            new SpecialItemDto { Type = "episode", Id = 1658202 },
            new SpecialItemDto { Type = "episode", Id = 1675728 },
            new SpecialItemDto { Type = "episode", Id = 1675729 },
            new SpecialItemDto { Type = "episode", Id = 1675730 },
            new SpecialItemDto { Type = "episode", Id = 1675731 },
            new SpecialItemDto { Type = "episode", Id = 1675732 },
            new SpecialItemDto { Type = "episode", Id = 1675733 },
            new SpecialItemDto { Type = "episode", Id = 1675734 },
            new SpecialItemDto { Type = "episode", Id = 1675735 },
            new SpecialItemDto { Type = "episode", Id = 1675736 },
            new SpecialItemDto { Type = "episode", Id = 1675737 },
            new SpecialItemDto { Type = "episode", Id = 1675738 },
            new SpecialItemDto { Type = "episode", Id = 1675739 },
            new SpecialItemDto { Type = "episode", Id = 1377164 },
            new SpecialItemDto { Type = "episode", Id = 1378310 },
            new SpecialItemDto { Type = "episode", Id = 1397002 },
            new SpecialItemDto { Type = "episode", Id = 1397003 },
            new SpecialItemDto { Type = "episode", Id = 1397004 },
            new SpecialItemDto { Type = "episode", Id = 1407271 },
            new SpecialItemDto { Type = "episode", Id = 1408720 },
            new SpecialItemDto { Type = "episode", Id = 1411774 },
            new SpecialItemDto { Type = "episode", Id = 1416888 },
            new SpecialItemDto { Type = "episode", Id = 1418736 },
            new SpecialItemDto { Type = "episode", Id = 1431231 },
            new SpecialItemDto { Type = "episode", Id = 1434147 },
            new SpecialItemDto { Type = "episode", Id = 1437458 },
            new SpecialItemDto { Type = "episode", Id = 1445522 },
            new SpecialItemDto { Type = "episode", Id = 1449541 },
            new SpecialItemDto { Type = "episode", Id = 1455051 },
            new SpecialItemDto { Type = "episode", Id = 1459965 },
            new SpecialItemDto { Type = "episode", Id = 1462484 },
            new SpecialItemDto { Type = "episode", Id = 1467645 },
            new SpecialItemDto { Type = "episode", Id = 1472410 },
            new SpecialItemDto { Type = "episode", Id = 1472843 },
            new SpecialItemDto { Type = "episode", Id = 1472849 },
            new SpecialItemDto { Type = "episode", Id = 1526698 },
            new SpecialItemDto { Type = "episode", Id = 1534349 },
            new SpecialItemDto { Type = "episode", Id = 1534350 },
            new SpecialItemDto { Type = "episode", Id = 1534351 },
            new SpecialItemDto { Type = "episode", Id = 1534352 },
            new SpecialItemDto { Type = "episode", Id = 1534353 },
            new SpecialItemDto { Type = "episode", Id = 1534354 },
            new SpecialItemDto { Type = "episode", Id = 1534355 },
            new SpecialItemDto { Type = "episode", Id = 1534356 },
            new SpecialItemDto { Type = "episode", Id = 1534357 },
            new SpecialItemDto { Type = "movie", Id = 363088 },
            new SpecialItemDto { Type = "movie", Id = 299536 },
            new SpecialItemDto { Type = "movie", Id = 299534 },
            new SpecialItemDto { Type = "episode", Id = 1830976 },
            new SpecialItemDto { Type = "episode", Id = 2293605 },
            new SpecialItemDto { Type = "episode", Id = 2639816 },
            new SpecialItemDto { Type = "episode", Id = 2639817 },
            new SpecialItemDto { Type = "episode", Id = 2639818 },
            new SpecialItemDto { Type = "episode", Id = 2639819 },
            new SpecialItemDto { Type = "episode", Id = 2639820 },
            new SpecialItemDto { Type = "episode", Id = 2639821 },
            new SpecialItemDto { Type = "episode", Id = 2724621 },
            new SpecialItemDto { Type = "episode", Id = 2431898 },
            new SpecialItemDto { Type = "episode", Id = 2535021 },
            new SpecialItemDto { Type = "episode", Id = 2535022 },
            new SpecialItemDto { Type = "episode", Id = 2558741 },
            new SpecialItemDto { Type = "episode", Id = 2558742 },
            new SpecialItemDto { Type = "episode", Id = 2558743 },
            new SpecialItemDto { Type = "movie", Id = 429617 },
            new SpecialItemDto { Type = "episode", Id = 2534997 },
            new SpecialItemDto { Type = "episode", Id = 2927202 },
            new SpecialItemDto { Type = "episode", Id = 2927203 },
            new SpecialItemDto { Type = "episode", Id = 2927204 },
            new SpecialItemDto { Type = "episode", Id = 2927205 },
            new SpecialItemDto { Type = "episode", Id = 2927206 },
        ];

        Special specialDto = new Special
        {
            Id = Ulid.Parse("01HSBYSE7ZNGN7P586BQJ7W9ZB"),
            Title = "Marvel Cinematic Universe",
            Backdrop = "https://storage.nomercy.tv/laravel/clje9xd4v0000d4ef0usufhy9.jpg",
            Poster = "/4Af70wDv1sN8JztUNnvXgae193O.jpg",
            Logo = "/hUzeosd33nzE5MCNsZxCGEKTXaQ.png",
            Description = "Chronological order of the movies and episodes from the Marvel Cinematic Universe in the timeline of the story.",
            Creator = "Stoney_Eagle",
        };

        await MediaContext.Specials
            .Upsert(specialDto)
            .On(v => new { v.Id })
            .WhenMatched((si, su) => new Special()
            {
                Id = su.Id,
                Title = su.Title,
                Backdrop = su.Backdrop,
                Poster = su.Poster,
                Logo = su.Logo,
                Description = su.Description,
                Creator = su.Creator,
            })
            .RunAsync();

        foreach (var specialItem in specialData)
        {
            int index = specialData.IndexOf(specialItem);

            try
            {
                if (specialItem.Type == "movie" )
                {
                    await MediaContext.SpecialItems
                        .Upsert(new SpecialItem()
                        {
                            SpecialId = specialDto.Id,
                            Order = index,
                            MovieId = specialItem.Id,
                        })
                        .On(v => new { v.SpecialId, v.MovieId })
                        .WhenMatched((si, su) => new SpecialItem()
                        {
                            SpecialId = su.SpecialId,
                            Order = su.Order,
                            MovieId = su.MovieId,
                        })
                        .RunAsync();
                }
                else
                {
                    await MediaContext.SpecialItems
                        .Upsert(new SpecialItem()
                        {
                            SpecialId = specialDto.Id,
                            Order = index,
                            EpisodeId = specialItem.Id,
                        })
                        .On(v => new { v.SpecialId, v.EpisodeId })
                        .WhenMatched((si, su) => new SpecialItem()
                        {
                            SpecialId = su.SpecialId,
                            Order = su.Order,
                            EpisodeId = su.EpisodeId,
                        })
                        .RunAsync();
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}

public class ServerUserDto
{
    [JsonProperty("user_id")] public Guid UserId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("enabled")] public bool Enabled { get; set; }
    [JsonProperty("cache_id")] public string CacheId { get; set; }
    [JsonProperty("avatar")] public Uri Avatar { get; set; }
    [JsonProperty("is_owner")] public bool IsOwner { get; set; }
}

public class LibrarySeedDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("image")] public string Image { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("order")] public int Order { get; set; } = 99;
    [JsonProperty("specialSeasonName")] public string SpecialSeasonName { get; set; }
    [JsonProperty("realtime")] public bool Realtime { get; set; }
    [JsonProperty("autoRefreshInterval")] public int AutoRefreshInterval { get; set; }
    [JsonProperty("chapterImages")] public bool ChapterImages { get; set; }

    [JsonProperty("extractChaptersDuring")]
    public bool ExtractChaptersDuring { get; set; }

    [JsonProperty("extractChapters")] public bool ExtractChapters { get; set; }
    [JsonProperty("perfectSubtitleMatch")] public bool PerfectSubtitleMatch { get; set; }
    [JsonProperty("folders")] public FolderDto[] Folders { get; set; }
}

public class FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
}

public class EncoderProfileDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("container")] public string Container { get; set; }
    [JsonProperty("params")] public EncoderProfileParamsDto Params { get; set; }
}

public class EncoderProfileParamsDto
{
    [JsonProperty("width")] public int Width { get; set; }
    [JsonProperty("crf")] public int Crf { get; set; }
    [JsonProperty("preset")] public string Preset { get; set; }
    [JsonProperty("profile")] public string Profile { get; set; }
    [JsonProperty("codec")] public string Codec { get; set; }
    [JsonProperty("audio")] public string Audio { get; set; }
}

public class SpecialItemDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
}

public class SpecialDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("backdrop")] public string Backdrop { get; set; }
    [JsonProperty("poster")] public string Poster { get; set; }
    [JsonProperty("logo")] public string Logo { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("Item")] public SpecialItemDto[] Item { get; set; }
    [JsonProperty("creator")] public string Creator { get; set; }
}