using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Server.Helpers;
using Genre = NoMercy.Database.Models.Genre;

namespace NoMercy.Server.Logic;

public class Seed
{
    private ConfigClient ConfigClient { get; set; } = new();
    private MovieClient MovieClient { get; set; } = new();
    private TvClient TvClient { get; set; } = new();
    
    public Seed()
    {
        CreateDatabase().Wait();
        SeedDatabase().Wait();
    }
    
    private static async Task CreateDatabase()
    {
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Database.EnsureCreatedAsync();
        await mediaContext.SaveChangesAsync();
    }

    private async Task SeedDatabase()
    {
        try
        {
            await AddGenres();
            await AddCertifications();
            await AddLanguages();
            await AddCountries();
            await AddMusicGenres();
            await AddFolderRoots();
            await AddLibraries();
            await GetUsers();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
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
        
        if (content == null) throw new Exception("Failed to get server info");

        ServerUser[] serverUsers = JsonConvert.DeserializeObject<ServerUser[]>(content) ?? [];
        
        User[] users = serverUsers.ToList().ConvertAll<User>(serverUser => new User()
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
        }).ToArray();
        
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Users.UpsertRange(users)
            .On(v => new { v.Id })
            .WhenMatched((us, ui) => new User()
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
    }
    

    private async Task AddGenres()
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
        
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Genres.UpsertRange(genres)
            .On(v => new { v.Id })
            .WhenMatched(v => new Genre()
            {
                Id = v.Id,
                Name = v.Name
            })
            .RunAsync();
    }

    private async Task AddCertifications()
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
        
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Certifications.UpsertRange(certifications)
            .On(v => new { v.Iso31661, v.Rating })
            .WhenMatched(v => new Certification()
            {
                Iso31661 = v.Iso31661,
                Rating = v.Rating,
                Meaning = v.Meaning,
                Order = v.Order
            })
            .RunAsync();
    }

    private async Task AddLanguages()
    {
        var languages = ConfigClient.Languages().Result.ToList()
            .ConvertAll<Language>(language => new Language(language)).ToArray();
            
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Languages.UpsertRange(languages)
            .On(v => new { v.Iso6391 })
            .WhenMatched(v => new Language()
            {
                Iso6391 = v.Iso6391,
                Name = v.Name,
                EnglishName = v.EnglishName,
            })
            .RunAsync();
    }

    private async Task AddCountries()
    {
        var countries = ConfigClient.Countries().Result.ToList()
            .ConvertAll<Country>(country => new Country(country)).ToArray();
        
        await using MediaContext mediaContext = new MediaContext();
        await mediaContext.Countries.UpsertRange(countries)
            .On(v => new { v.Iso31661 })
            .WhenMatched(v => new Country()
            {
                Iso31661 = v.Iso31661,
                NativeName = v.NativeName,
                EnglishName = v.EnglishName,
            })
            .RunAsync();
    
    }

    private async Task AddMusicGenres()
    {
        await Task.CompletedTask;
        // var musicGenres = ConfigClient.MusicGenres().Result;

    }

    private async Task AddFolderRoots()
    {
        try
        {
            var folders = JsonConvert.DeserializeObject<Folder[]>(await System.IO.File.ReadAllTextAsync(AppFiles.FolderRootsSeedFile)) ?? [];
            
            await using MediaContext mediaContext = new MediaContext();
            await mediaContext.Folders.UpsertRange(folders)
                .On(v => new { v.Id })
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

    private async Task AddLibraries()
    {
        try
        {
            Library[] libraries = JsonConvert.DeserializeObject<Library[]>(await System.IO.File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? [];
            
            await using MediaContext mediaContext = new MediaContext();
            await mediaContext.Libraries.UpsertRange(libraries)
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
                    Country = vi.Country,
                    Language = vi.Language,
                })
                .RunAsync();
                
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

public class ServerUser
{
    [JsonProperty("user_id")]
    public Guid UserId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("cache_id")]
    public string CacheId { get; set; }

    [JsonProperty("avatar")]
    public Uri Avatar { get; set; }

    [JsonProperty("is_owner")]
    public bool IsOwner { get; set; }
}
