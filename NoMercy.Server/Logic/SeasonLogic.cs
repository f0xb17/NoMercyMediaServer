using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;
using Season = NoMercy.Database.Models.Season;

namespace NoMercy.Server.Logic;

public class SeasonLogic(TmdbTvShowDetails show) : IDisposable, IAsyncDisposable
{
    private readonly ConcurrentStack<TmdbSeasonAppends> _seasonAppends = [];

    public async Task FetchSeasons()
    {
        foreach (var season in show.Seasons)
            try
            {
                await using TmdbSeasonClient tmdbSeasonClient = new(show.Id, season.SeasonNumber);
                var seasonTask = await tmdbSeasonClient.WithAllAppends();
                if (seasonTask is null) continue;
                _seasonAppends.Push(seasonTask);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }

        await Store();

        await StoreTranslations();

        await StoreEpisodes();

        await DispatchJobs();
    }

    private Task DispatchJobs()
    {
        return Task.CompletedTask;
    }

    private Task Store()
    {
        try
        {
            Season[] seasons = _seasonAppends.ToList()
                .ConvertAll<Season>(x => new Season(x, show.Id)
                {
                    _colorPalette = MovieDbImage.ColorPalette("poster", x.PosterPath).Result
                })
                .ToArray();

            using MediaContext mediaContext = new();
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
                    _colorPalette = si._colorPalette
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
        foreach (var season in _seasonAppends)
        {
            using EpisodeLogic episodeLogic = new(show, season);
            episodeLogic.FetchEpisodes().Wait();
        }

        return Task.CompletedTask;
    }

    public static async Task Palette(int id)
    {
        Logger.Queue($"Fetching color palette for Season {id}");

        await using MediaContext mediaContext = new();

        var season = await mediaContext.Seasons
            .Where(e => e._colorPalette == "")
            .FirstOrDefaultAsync(e => e.Id == id);

        if (season is { _colorPalette: "" })
        {
            season._colorPalette = await MovieDbImage.ColorPalette("poster", season.Poster);

            await mediaContext.SaveChangesAsync();
        }

        Image[] images = await mediaContext.Images
            .Where(e => e.SeasonId == id)
            .Where(e => e._colorPalette == "")
            .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
                        e.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            .ToArrayAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;

            Logger.Queue($"Fetching color palette for Season Image {image.FilePath}");

            ColorPaletteJob colorPaletteJob = new(image.FilePath, "image", image.Iso6391);
            JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        }
    }

    public static async Task StoreImages(int show, int id)
    {
        await using MediaContext mediaContext = new();

        var tvShow = await mediaContext.Tvs
            .FirstOrDefaultAsync(tv => tv.Id == show);

        TmdbSeasonClient tmdbSeasonClient = new(show, id);
        var season = await tmdbSeasonClient.WithAllAppends();
        if (season is null) return;

        List<Image> images = season.TmdbSeasonImages.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image, season, "poster"));

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
                SeasonId = ti.SeasonId
            })
            .RunAsync();

        ColorPaletteJob colorPaletteJob = new(season.Id, "season");
        JobDispatcher.Dispatch(colorPaletteJob, "image", 2);

        Logger.MovieDb($@"TvShow {tvShow?.Title}, Season {season.SeasonNumber}: Images stored");
    }

    private Task StoreTranslations()
    {
        using MediaContext mediaContext = new();
        foreach (var season in _seasonAppends)
        {
            Translation[] translations = season.Translations.Translations.ToList()
                .ConvertAll<Translation>(x => new Translation(x, season)).ToArray();

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
                    PersonId = ti.PersonId
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

    public async ValueTask DisposeAsync()
    {
        _seasonAppends.Clear();
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}