using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;
using Season = NoMercy.Database.Models.Season;

namespace NoMercy.Server.Logic;

public class SeasonLogic(TvShowAppends show)
{
    private readonly ConcurrentStack<SeasonAppends> _seasonAppends = [];

    public async Task FetchSeasons()
    {
        foreach (Providers.TMDB.Models.Season.Season season in show.Seasons)
        {
            try
            {
                using SeasonClient seasonClient = new(show.Id, season.SeasonNumber);
                SeasonAppends? seasonTask = await seasonClient.WithAllAppends();
                if (seasonTask is null) continue;
                _seasonAppends.Push(seasonTask);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }

        await Store();
        
        await StoreTranslations();
        
        await StoreEpisodes();
        
        await DispatchJobs();
    }

    private Task DispatchJobs()
    {
        foreach (SeasonAppends season in _seasonAppends)
        {
            try
            {
                SeasonImagesJob imagesJob = new SeasonImagesJob(id: season.Id, seasonNumber: season.SeasonNumber);
                JobDispatcher.Dispatch(imagesJob, "data", 2);
            }
            catch (Exception e)
            {
                Logger.Encoder(e, LogLevel.Error);
            }
        }

        return Task.CompletedTask;
    }

    private Task Store()
    {
        try
        {
            Season[] seasons = _seasonAppends.ToList()
                .ConvertAll<Season>(x => new Season(x, show.Id))
                .ToArray();

            using MediaContext mediaContext = new MediaContext();
            mediaContext.Seasons.UpsertRange(seasons)
                .On(s => new { s.Id })
                .WhenMatched((ss, si) => new Season
                {
                    Id = si.Id,
                    Title = si.Title,
                    AirDate = si.AirDate,
                    EpisodeCount = si.EpisodeCount,
                    Overview = si.Overview,
                    Poster = si.Poster,
                    SeasonNumber = si.SeasonNumber,
                    TvId = si.TvId,
                    _colorPalette = si._colorPalette,
                })
                .Run();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        Logger.MovieDb($@"TvShow {show.Name}: Seasons stored");
        
        return Task.CompletedTask;
    }
    
    private Task StoreEpisodes()
    {
        Parallel.ForEach(_seasonAppends, (season) =>
        {
            EpisodeLogic episodeLogic = new(show, season);
            episodeLogic.FetchEpisodes().Wait();
            episodeLogic.Dispose();
        });

        return Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        Season? season = await mediaContext.Seasons
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (season is { _colorPalette: "", Poster: not null })
        {
            string palette = await ImageLogic.GenerateColorPalette(posterPath: season.Poster);
            season._colorPalette = palette;
            
            await mediaContext.SaveChangesAsync();
        }
    }

    public static async Task StoreImages(int show, int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        Tv? tvShow = await mediaContext.Tvs
            .FirstOrDefaultAsync(tv => tv.Id == show);

        SeasonClient seasonClient = new(show, id);
        SeasonAppends? season = await seasonClient.WithAllAppends();
        if (season is null) return;

        List<Image> images = season.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image: image, season: season, type: "poster"));

        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.SeasonId })
            .WhenMatched((ts, ti) => new Image
            {
                AspectRatio = ti.AspectRatio,
                FilePath = ti.FilePath,
                Height = ti.Height,
                Iso6391 = ti.Iso6391,
                Site = ti.Site,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Width = ti.Width,
                Type = ti.Type,
                SeasonId = ti.SeasonId,
            })
            .RunAsync();
        
        try
        {
            await new ColorPaletteJob(id: season.Id, model: "season").Handle();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        foreach (Image image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            try
            {
                await new ColorPaletteJob(filePath: image.FilePath, model: "image", image.Iso6391).Handle();
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }

        Logger.MovieDb($@"TvShow {tvShow?.Title}, Season {season.SeasonNumber}: Images stored");
        await Task.CompletedTask;
    }

    private Task StoreTranslations()
    {
        foreach (SeasonAppends season in _seasonAppends)
        {
            Translation[] translations = season.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, season)).ToArray();

            using MediaContext mediaContext = new MediaContext();
            mediaContext.Translations
                .UpsertRange(translations.Where(translation =>
                    translation.Title != null || translation.Overview != ""))
                .On(t => new { t.Iso31661, t.Iso6391, t.SeasonId })
                .WhenMatched((ts, ti) => new Translation
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
                .Run();

            Logger.MovieDb($@"TvShow {show?.Name}, Season {season.SeasonNumber}: Translations stored");
        }
    
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _seasonAppends.Clear();
        
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}