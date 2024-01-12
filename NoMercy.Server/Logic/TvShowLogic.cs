using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;
using Person = NoMercy.Database.Models.Person;
using Translation = NoMercy.Database.Models.Translation;

namespace NoMercy.Server.Logic;

public class TvShowLogic
{
    private TvClient TvClient { get; set; }
    private Library Library { get; set; }
    private string MediaType { get; set; } = null!;
    private string Folder { get; set; } = null!;

    public TvShowAppends? Show { get; set; }
    
    public TvShowLogic(int id, Library library)
    {
        Library = library;
        TvClient = new TvClient(id);
    }

    public async Task Process()
    {
        var tv = MediaContext.Db!.Tvs.FirstOrDefaultAsync(t => t.Id == TvClient.Id).Result;
        if (tv != null)
        {
            Console.WriteLine(@"TvShow {0} already exists", tv.Title);
            return;
        }
        
        Show = TvClient!.WithAllAppends().Result;

        Folder = FindFolder();
        MediaType = GetMediaType();

        await Store();
        
        await SeasonLogic.FetchSeasons(Show);
        Console.WriteLine(@"Fetched Seasons for {0}", Show.Name);
        
        await PersonLogic.FetchPeople(Show);
        Console.WriteLine(@"Fetched People for {0}", Show.Name);
        //
        // await FetchGenres();
        // Console.WriteLine(@"Fetched Genres for {0}", Show.Name);
        //
        // await FetchKeywords();
        // Console.WriteLine(@"Fetched Keywords for {0}", Show.Name);
        //
        // await FetchAggregateCredits();
        // Console.WriteLine(@"Fetched AggregateCredits for {0}", Show.Name);
        //
        await FetchAlternativeTitles();
        Console.WriteLine(@"Fetched AlternativeTitles for {0}", Show.Name);
        //
        // await FetchCompanies();
        // Console.WriteLine(@"Fetched Companies for {0}", Show.Name);
        //
        // await FetchNetworks();
        // Console.WriteLine(@"Fetched Networks for {0}", Show.Name);
        //
        await FetchCast();
        Console.WriteLine(@"Fetched Cast for {0}", Show.Name);
        
        await FetchCrew();
        Console.WriteLine(@"Fetched Crew for {0}", Show.Name);
        
        // await FetchImages();
        // Console.WriteLine(@"Fetched Images for {0}", Show.Name);
        
        // await FetchVideos();
        // Console.WriteLine(@"Fetched Videos for {0}", Show.Name);
        
        // await FetchRecommendations();
        // Console.WriteLine(@"Fetched Recommendations for {0}", Show.Name);
        
        // await FetchSimilar();
        // Console.WriteLine(@"Fetched Similar for {0}", Show.Name);
        
        // await FetchContentRatings();
        // Console.WriteLine(@"Fetched ContentRatings for {0}", Show.Name);
        
        await FetchTranslations();
        Console.WriteLine(@"Fetched Translations for {0}", Show.Name);
        
        // await FetchWatchProviders();
        // Console.WriteLine(@"Fetched WatchProviders for {0}", Show.Name);
        
    }

    private async Task FetchAlternativeTitles()
    {
        var alternativeTitles = Show!.AlternativeTitles!.Results.ToList()
            .ConvertAll<AlternativeTitle>(x => new AlternativeTitle(x)).ToArray();
        
        await MediaContext.Db.AlternativeTitles
            .UpsertRange(alternativeTitles)
            .On(a => new { a.Title })
            .WhenMatched((ats, ati) => new AlternativeTitle()
            {
                Title = ati.Title,
                Iso31661 = ati.Iso31661,
            })
            .RunAsync();
    }

    private async Task FetchAggregateCredits()
    {
        await Task.CompletedTask;
    }

    private async Task FetchCast()
    {
        try
        {
            var cast = Show!.Credits!.Cast.ToList()
                .ConvertAll<Cast>(x => new Cast(x, Show!)).ToArray();

            await MediaContext.Db.Casts
                .UpsertRange(cast)
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task FetchCrew()
    {
        try
        {
            var crew = Show!.Credits!.Crew
                .ConvertAll<Crew>(x => new Crew(x, Show!)).ToArray();
        
            await MediaContext.Db.Crews
                .UpsertRange(crew)
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task FetchWatchProviders()
    {
        await Task.CompletedTask;
    }

    private async Task FetchTranslations()
    {
        try
        {
            var translations = Show!.Translations!.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, Show!)).ToArray();
        
            await MediaContext.Db.Translations
                .UpsertRange(translations)
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
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task FetchContentRatings()
    {
        await Task.CompletedTask;
    }

    private async Task FetchSimilar()
    {
        await Task.CompletedTask;
    }

    private async Task FetchRecommendations()
    {
        await Task.CompletedTask;
    }

    private async Task FetchVideos()
    {
        await Task.CompletedTask;
    }

    private async Task FetchImages()
    {
        await Task.CompletedTask;
    }

    private async Task FetchNetworks()
    {
        await Task.CompletedTask;
    }

    private async Task FetchCompanies()
    {
        await Task.CompletedTask;
    }

    private async Task FetchKeywords()
    {
        await Task.CompletedTask;
    }

    private async Task FetchGenres()
    {
        await Task.CompletedTask;
    }

    private static string GetMediaType()
    {
        const string defaultMediaType = "tv";

        return defaultMediaType;
    }

    private static string FindFolder()
    {
        const string defaultFolder = "/American.Dad.(2005)";

        return defaultFolder;
    }
    
    private async Task Store()
    {
        Tv tvResponse = new Tv(Show!, Library.Id, Folder, MediaType);
        Console.WriteLine(@"Storing TvShow {0}", Show!.Name);

        await MediaContext.Db.Tvs.Upsert(tvResponse)
            .On(v => new { v.Id })
            .WhenMatched(v => new Tv()
            {
                Id = tvResponse.Id,
                Backdrop = tvResponse.Backdrop,
                Duration = tvResponse.Duration,
                FirstAirDate = tvResponse.FirstAirDate,
                Homepage = tvResponse.Homepage,
                ImdbId = tvResponse.ImdbId,
                InProduction = tvResponse.InProduction,
                LastEpisodeToAir = tvResponse.LastEpisodeToAir,
                NextEpisodeToAir = tvResponse.NextEpisodeToAir,
                NumberOfEpisodes = tvResponse.NumberOfEpisodes,
                NumberOfSeasons = tvResponse.NumberOfSeasons,
                OriginCountry = tvResponse.OriginCountry,
                OriginalLanguage = tvResponse.OriginalLanguage,
                Overview = tvResponse.Overview,
                Popularity = tvResponse.Popularity,
                Poster = tvResponse.Poster,
                SpokenLanguages = tvResponse.SpokenLanguages,
                Status = tvResponse.Status,
                Tagline = tvResponse.Tagline,
                Title = tvResponse.Title,
                TitleSort = tvResponse.TitleSort,
                Trailer = tvResponse.Trailer,
                TvdbId = tvResponse.TvdbId,
                Type = tvResponse.Type,
                VoteAverage = tvResponse.VoteAverage,
                VoteCount = tvResponse.VoteCount,
            })
            .RunAsync();
        
        Console.WriteLine(@"Stored TvShow {0}", Show.Name);
        
    }
}