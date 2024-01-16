using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Helpers;
using NoMercy.Server.Jobs;
using LogLevel = NoMercy.Helpers.LogLevel;
using Person = NoMercy.Database.Models.Person;
using Translation = NoMercy.Database.Models.Translation;

namespace NoMercy.Server.Logic;

public class TvShowLogic(int id, Library library)
{
    private TvClient TvClient { get; } = new(id);
    private string MediaType { get; set; } = null!;
    private string Folder { get; set; } = null!;

    public TvShowAppends? Show { get; set; }

    public async Task Process()
    {
        // await using MediaContext mediaContext = new MediaContext();
        
        // await mediaContext.Tvs.FirstOrDefaultAsync(t => t.Id == TvClient.Id);
        // if (tv != null)
        // {
        //     Logger.MovieDb($@"TvShow {Show!.Name} already exists", tv.Title);
        //     return;
        // }

        Show = await TvClient.WithAllAppends();
        if (Show == null)
        {
            Logger.MovieDb($@"TvShow {TvClient.Id} not found");
            return;
        }
        
        Folder = FindFolder();
        Logger.MovieDb(Folder, LogLevel.Info);
        MediaType = GetMediaType();
        
        await Store();
        
        PersonLogic personLogic = new PersonLogic(Show);
        await personLogic.FetchPeople();
        personLogic.Dispose();
        
        
        // await StoreGenres();
        // Logger.MovieDb($@"TvShow {Show!.Name} Genres stored");
        
        // await StoreKeywords();
        // Logger.MovieDb($@"TvShow {Show!.Name} Keywords stored");
        
        // await StoreAggregateCredits();
        // Logger.MovieDb($@"TvShow {Show!.Name} AggregateCredits stored");
        
        await StoreAlternativeTitles();
        //
        // await StoreCompanies();
        // Logger.MovieDb($@"TvShow {Show!.Name} Companies stored");
        
        // await StoreNetworks();
        // Logger.MovieDb($@"TvShow {Show!.Name} Networks stored");
        
        await StoreCast();
        
        await StoreCrew();
        
        // await StoreImages();
        // Logger.MovieDb($@"TvShow {Show!.Name} Images stored");
        
        // await StoreVideos();
        // Logger.MovieDb($@"TvShow {Show!.Name} Videos stored");
        
        // await StoreRecommendations();
        // Logger.MovieDb($@"TvShow {Show!.Name} Recommendations stored");
        
        // await StoreSimilar();
        // Logger.MovieDb($@"TvShow {Show!.Name} Similar stored");
        
        // await StoreContentRatings();
        // Logger.MovieDb($@"TvShow {Show!.Name} ContentRatings stored");
        
        await StoreTranslations();
        
        // await StoreWatchProviders();
        // Logger.MovieDb($@"TvShow {Show.Name} WatchProviders stored");
        
        await DispatchJobs();
        
        SeasonLogic seasonLogic = new SeasonLogic(Show);
        await seasonLogic.FetchSeasons();
        seasonLogic.Dispose();

    }

    private async Task StoreAlternativeTitles()
    {
        var alternativeTitles = Show!.AlternativeTitles.Results.ToList()
            .ConvertAll<AlternativeTitle>(x => new AlternativeTitle(x)).ToArray();
        
        await using MediaContext mediaContext = new MediaContext();
        
        await mediaContext.AlternativeTitles.UpsertRange(alternativeTitles)
            .On(a => new { a.Title })
            .WhenMatched((ats, ati) => new AlternativeTitle()
            {
                Title = ati.Title,
                Iso31661 = ati.Iso31661,
            })
            .RunAsync();
        
        Logger.MovieDb($@"TvShow {Show!.Name} AlternativeTitles stored");
    }

    private async Task StoreAggregateCredits()
    {
        await Task.CompletedTask;
    }

    private async Task StoreCast()
    {
        try
        {
            var cast = Show!.Credits.Cast.ToList()
                .ConvertAll<Cast>(x => new Cast(x, Show!)).ToArray();

            await using MediaContext mediaContext = new MediaContext();
        
            await mediaContext.Casts.UpsertRange(cast)
                .On(c => new { c.Id })
                .WhenMatched((cs, ci) => new Cast()
                {
                    Id = ci.Id,
                    PersonId = ci.PersonId,
                    MovieId = ci.MovieId,
                    TvId = ci.TvId,
                    SeasonId = ci.SeasonId,
                    EpisodeId = ci.EpisodeId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"TvShow {Show!.Name} Cast stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
            throw;
        }
    }
    
    private async Task StoreCrew()
    {
        try
        {
            var crew = Show!.Credits.Crew.ToList()
                .ConvertAll<Crew>(x => new Crew(x, Show!)).ToArray();
        
            await using MediaContext mediaContext = new MediaContext();
        
            await mediaContext.Crews.UpsertRange(crew)
                .On(c => new { c.Id })
                .WhenMatched((cs, ci) => new Crew()
                {
                    Id = ci.Id,
                    PersonId = ci.PersonId,
                    MovieId = ci.MovieId,
                    TvId = ci.TvId,
                    SeasonId = ci.SeasonId,
                    EpisodeId = ci.EpisodeId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"TvShow {Show!.Name} Crew stored", LogLevel.Info);
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
            throw;
        }
    }

    private async Task StoreWatchProviders()
    {
        await Task.CompletedTask;
    }

    private async Task StoreTranslations()
    {
        try
        {
            var translations = Show!.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, Show!)).ToArray() ?? [];
        
            await using MediaContext mediaContext = new MediaContext();
        
            await mediaContext.Translations.UpsertRange(translations)
                .On(t => new { t.Iso31661, t.Iso6391, t.TvId })
                .WhenMatched((ts, ti) => new Translation()
                {
                    Iso31661 = ti.Iso31661,
                    Iso6391 = ti.Iso6391,
                    Name = ti.Name,
                    EnglishName = ti.EnglishName,
                    Title = ti.Title,
                    Overview = ti.Overview,
                    Homepage = ti.Homepage,
                    Biography = ti.Biography,
                    TvId = ti.TvId,
                    SeasonId = ti.SeasonId,
                    EpisodeId = ti.EpisodeId,
                    MovieId = ti.MovieId,
                    CollectionId = ti.CollectionId,
                    PersonId = ti.PersonId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
            throw;
        }
    }

    private async Task StoreContentRatings()
    {
        await Task.CompletedTask;
    }

    private async Task StoreSimilar()
    {
        await Task.CompletedTask;
    }

    private async Task StoreRecommendations()
    {
        await Task.CompletedTask;
    }

    private async Task StoreVideos()
    {
        await Task.CompletedTask;
    }

    private async Task StoreImages()
    {
        await Task.CompletedTask;
    }

    private async Task StoreNetworks()
    {
        await Task.CompletedTask;
    }

    private async Task StoreCompanies()
    {
        await Task.CompletedTask;
    }

    private async Task StoreKeywords()
    {
        await Task.CompletedTask;
    }

    private async Task StoreGenres()
    {
        await Task.CompletedTask;
    }

    private static string GetMediaType()
    {
        const string defaultMediaType = "tv";

        return defaultMediaType;
    }

    private string FindFolder()
    {
        return FileNameParsers.CreateBaseFolder(Show!);
    }
    
    private async Task Store()
    {
        if (Show == null)
        {
            Logger.MovieDb($@"TvShow {TvClient.Id} not found");
            return;
        }
        
        Tv tvResponse = new Tv(Show, library.Id, Folder, MediaType);

        await using MediaContext mediaContext = new MediaContext();
        
        await mediaContext.Tvs.Upsert(tvResponse)
            .On(v => new { v.Id })
            .WhenMatched((ts, ti) => new Tv()
            {
                Id = ti.Id,
                Backdrop = ti.Backdrop,
                Duration = ti.Duration,
                FirstAirDate = ti.FirstAirDate,
                Homepage = ti.Homepage,
                ImdbId = ti.ImdbId,
                InProduction = ti.InProduction,
                LastEpisodeToAir = ti.LastEpisodeToAir,
                NextEpisodeToAir = ti.NextEpisodeToAir,
                NumberOfEpisodes = ti.NumberOfEpisodes,
                NumberOfSeasons = ti.NumberOfSeasons,
                OriginCountry = ti.OriginCountry,
                OriginalLanguage = ti.OriginalLanguage,
                Overview = ti.Overview,
                Popularity = ti.Popularity,
                Poster = ti.Poster,
                SpokenLanguages = ti.SpokenLanguages,
                Status = ti.Status,
                Tagline = ti.Tagline,
                Title = ti.Title,
                TitleSort = ti.TitleSort,
                Trailer = ti.Trailer,
                TvdbId = ti.TvdbId,
                Type = ti.Type,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Folder = ti.Folder,
                LibraryId = ti.LibraryId,
                MediaType = ti.MediaType,
                _colorPalette = ti._colorPalette,
                UpdatedAt = ti.UpdatedAt,
            })
            .RunAsync();
        
        Logger.MovieDb($@"TvShow {Show.Name} TvShow stored");
    }

    private Task DispatchJobs()
    {
        ColorPaletteJob colorPaletteJob = new ColorPaletteJob(id: Show.Id, model: "tv");
        JobDispatcher.Dispatch(colorPaletteJob, "data");
        
        return Task.CompletedTask;
    }
    
    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();
        

        var show = await mediaContext.Tvs
            .FirstOrDefaultAsync(e => e.Id == id);

        if (show is { _colorPalette: "" })
        {
            var palette = await ImageLogic.GenerateColorPalette(posterPath: show.Poster, backdropPath: show.Backdrop);
            show._colorPalette = palette;
            await mediaContext.SaveChangesAsync();
        }
    }
    
    public void Dispose()
    {
        Show = null;
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
    
}