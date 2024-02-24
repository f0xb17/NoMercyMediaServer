using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Server.app.Helper;
using Genre = NoMercy.Database.Models.Genre;

namespace NoMercy.Server.Logic;

public static class Seed
{
    private static ConfigClient ConfigClient { get; set; } = new();
    private static MovieClient MovieClient { get; set; } = new();
    private static TvClient TvClient { get; set; } = new();
    private static readonly MediaContext MediaContext = new();
    private static Folder[] _folders = [];
    private static User?[] _users = [];


    public static async Task Init()
    {
        await CreateDatabase();
        await SeedDatabase();
    }

    private static async Task CreateDatabase()
    {
        await MediaContext.Database.EnsureCreatedAsync();
        await MediaContext.SaveChangesAsync();
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
            await AddEncoerProfiles();
            await AddLibraries();
            await GetUsers();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task AddEncoerProfiles()
    {
        var encoderProfiles = JsonConvert.DeserializeObject<EncoderProfileDto[]>(
            await System.IO.File.ReadAllTextAsync(AppFiles.EncoderProfilesSeedFile)) ?? [];

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
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "NoMercy");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth.AccessToken);

        IDictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("server_id", SystemInfo.DeviceId);

        var newUrl = QueryHelpers.AddQueryString("https://api-dev.nomercy.tv/v1/server/users", query);

        var response = await client.GetAsync(newUrl);
        var content = await response.Content.ReadAsStringAsync();

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

        Library[] libraries =
            JsonConvert.DeserializeObject<Library[]>(
                await System.IO.File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? [];

        List<LibraryUser> libraryUsers = [];

        foreach (var user in _users.ToList())
        {
            foreach (var library in libraries.ToList())
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
            MovieClient.Genres()
                .Result.Genres.ToList()
                .ConvertAll<Genre>(x => new Genre(x)).ToArray()
        );

        genres.AddRange(
            TvClient.Genres()
                .Result.Genres.ToList()
                .ConvertAll<Genre>(x => new Genre(x)).ToArray()
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

        foreach (var keyValuePair in MovieClient.Certifications().Result.Certifications)
        {
            foreach (var certification in keyValuePair.Value)
            {
                certifications.Add(new Certification(keyValuePair.Key, certification));
            }
        }

        foreach (var keyValuePair in TvClient.Certifications().Result.Certifications)
        {
            foreach (var certification in keyValuePair.Value)
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
        var languages = ConfigClient.Languages().Result.ToList()
            .ConvertAll<Language>(language => new Language(language)).ToArray();

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
        var countries = ConfigClient.Countries().Result?.ToList()
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
        await Task.CompletedTask;
        // var musicGenres = ConfigClient.MusicGenres().Result;
    }

    private static async Task AddFolderRoots()
    {
        try
        {
            _folders = JsonConvert.DeserializeObject<Folder[]>(
                await System.IO.File.ReadAllTextAsync(AppFiles.FolderRootsSeedFile)) ?? [];

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
            Console.WriteLine(e);
            throw;
        }
    }

    private static async Task AddLibraries()
    {
        try
        {
            List<LibrarySeedDto> librarySeed =
                (JsonConvert.DeserializeObject<LibrarySeedDto[]>(
                    await System.IO.File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? Array.Empty<LibrarySeedDto>())
                .ToList()
                ?? [];

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

            foreach (var library in librarySeed.ToList())
            {
                foreach (var folder in library.Folders.ToList())
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
            Console.WriteLine(e);
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