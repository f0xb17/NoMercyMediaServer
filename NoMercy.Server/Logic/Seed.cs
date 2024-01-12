using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
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
        await MediaContext.Db.Database.EnsureCreatedAsync();
        await MediaContext.Db.SaveChangesAsync();
    }

    private async Task SeedDatabase()
    {
        await AddGenres();
        await AddCertifications();
        await AddLanguages();
        await AddCountries();
        await AddMusicGenres();
        // await AddLibraries();
        
    }

    private async Task AddGenres()
    {
        List<Genre> genres = [];
            
        foreach (var genre in MovieClient.Genres().Result.Genres)
        {
            genres.Add(new Genre()
            {
                Id = genre.Id,
                Name = genre.Name
            });
        }
        foreach (var genre in TvClient.Genres().Result.Genres)
        {
            genres.Add(new Genre()
            {
                Id = genre.Id,
                Name = genre.Name
            });
        }

        await MediaContext.Db.Genres.UpsertRange(genres)
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
        
        foreach (var certification in MovieClient.Certifications().Result.Certifications)
        {
            foreach (var certificationMovie in certification.Value)
            {
                certifications.Add(new Certification()
                {
                    Iso31661 = certification.Key,
                    Rating = certificationMovie.CertificationCertification,
                    Meaning = certificationMovie.Meaning,
                    Order = certificationMovie.Order
                });
            }
            
        }
        foreach (var certification in TvClient.Certifications().Result.Certifications)
        {
            foreach (var certificationTv in certification.Value)
            {
                certifications.Add(new Certification()
                {
                    Iso31661 = certification.Key,
                    Rating = certificationTv.CertificationCertification,
                    Meaning = certificationTv.Meaning,
                    Order = certificationTv.Order
                });
            }
            
        }
        
        await MediaContext.Db.Certifications.UpsertRange(certifications)
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
        var languages = await ConfigClient.Languages();
        foreach (var language in languages)
        {
            await MediaContext.Db.Languages.Upsert(new Language()
            {
                Iso6391 = language.Iso6391,
                Name = language.Name,
                EnglishName = language.EnglishName,                
            })
                .On(v => new { v.Iso6391 })
                .WhenMatched(v => new Language()
                {
                    Iso6391 = v.Iso6391,
                    Name = v.Name,
                    EnglishName = v.EnglishName,
                })
                .RunAsync();
        }
    }

    private async Task AddCountries()
    {
        var countries = await ConfigClient.Countries();
        foreach (var country in countries)
        {
            await MediaContext.Db.Countries.Upsert(new Country()
            {
                Iso31661 = country.Iso31661,
                NativeName = country.NativeName,
                EnglishName = country.EnglishName,              
            })
                .On(v => new { v.Iso31661 })
                .WhenMatched(v => new Country()
                {
                    Iso31661 = v.Iso31661,
                    NativeName = v.NativeName,
                    EnglishName = v.EnglishName,
                })
                .RunAsync();
        }
    }

    private async Task AddMusicGenres()
    {
        await Task.CompletedTask;
        // var musicGenres = ConfigClient.MusicGenres().Result;

    }

    private async Task AddLibraries()
    {
        try
        {
            List<Library> libraries = JsonConvert.DeserializeObject<List<Library>>(await System.IO.File.ReadAllTextAsync(AppFiles.LibrariesSeedFile)) ?? [];
            
            await MediaContext.Db.Libraries.UpsertRange(libraries)
                .On(v => new { v.Id })
                .WhenMatched(v => new Library()
                {
                    Id = v.Id,
                    AutoRefreshInterval = v.AutoRefreshInterval,
                    ChapterImages = v.ChapterImages,
                    ExtractChapters = v.ExtractChapters,
                    ExtractChaptersDuring = v.ExtractChaptersDuring,
                    Image = v.Image,
                    PerfectSubtitleMatch = v.PerfectSubtitleMatch,
                    Realtime = v.Realtime,
                    SpecialSeasonName = v.SpecialSeasonName,
                    Title = v.Title,
                    Type = v.Type,
                    Country = v.Country,
                    Language = v.Language,
                })
                .RunAsync();
                
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
