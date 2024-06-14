using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using Episode = NoMercy.Database.Models.Episode;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public class EpisodeLogic(TmdbTvShow show, TmdbSeasonDetails tmdbSeason) : IDisposable, IAsyncDisposable
{
    private readonly ConcurrentStack<TmdbEpisodeAppends> _episodeAppends = [];

    public async Task FetchEpisodes()
    {
        await Parallel.ForEachAsync(tmdbSeason.Episodes, async (episode, _) =>
        {
            try
            {
                await using TmdbEpisodeClient tmdbEpisodeClient = new(show.Id, episode.SeasonNumber, episode.EpisodeNumber);
                var episodeTask = await tmdbEpisodeClient.WithAllAppends();
                if (episodeTask is null) return;
                _episodeAppends.Push(episodeTask);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });

        await Store();

        await StoreTranslations();

        DispatchJobs();
    }

    private Task Store()
    {
        MediaContext mediaContext = new();
        try
        {
            Episode[] episodes = _episodeAppends.ToList()
                .ConvertAll<Episode>(episode => new Episode(episode, show.Id, tmdbSeason.Id)
                {
                    _colorPalette = MovieDbImage.ColorPalette("still", episode.StillPath).Result
                })
                .ToArray();

            mediaContext.Episodes.UpsertRange(episodes)
                .On(e => new { e.Id })
                .WhenMatched((es, ei) => new Episode
                {
                    Id = ei.Id,
                    Title = ei.Title,
                    AirDate = ei.AirDate,
                    EpisodeNumber = ei.EpisodeNumber,
                    Overview = ei.Overview,
                    ProductionCode = ei.ProductionCode,
                    SeasonNumber = ei.SeasonNumber,
                    Still = ei.Still,
                    TvId = ei.TvId,
                    SeasonId = ei.SeasonId,
                    _colorPalette = ei._colorPalette
                })
                .Run();

            Logger.MovieDb(
                $@"TvShow {show.Name}: Season {tmdbSeason.SeasonNumber}: Stored {tmdbSeason.Episodes.Length} episode{(tmdbSeason.Episodes.Length > 1 ? "s" : "")}");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        return Task.CompletedTask;
    }

    private void DispatchJobs()
    {
    }

    public static async Task Palette(int id)
    {
        Logger.Queue($"Fetching color palette for Episode {id}");

        await using MediaContext mediaContext = new();

        var episode = await mediaContext.Episodes
            .Where(e => e._colorPalette == "")
            .FirstOrDefaultAsync(e => e.Id == id);

        if (episode is { _colorPalette: "" })
        {
            episode._colorPalette = await MovieDbImage.ColorPalette("still", episode.Still);

            await mediaContext.SaveChangesAsync();
        }

        Image[] images = await mediaContext.Images
            .Where(e => e.EpisodeId == id)
            .Where(e => e._colorPalette == "")
            .Where(e => e.Iso6391 == null || e.Iso6391 == "en" || e.Iso6391 == "" ||
                        e.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            .ToArrayAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;

            Logger.Queue($"Fetching color palette for Episode Image {image.FilePath}");

            ColorPaletteJob colorPaletteJob = new(image.FilePath, "image", image.Iso6391);
            JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        }
    }

    public static async Task StoreImages(int show, int season, int episodeNumber)
    {
        try
        {
            await using MediaContext mediaContext = new();

            var tv = await mediaContext.Tvs
                .FirstOrDefaultAsync(t => t.Id == show);

            if (tv is null) return;

            TmdbEpisodeClient tmdbEpisodeClient = new(show, season, episodeNumber);
            var episode = await tmdbEpisodeClient.WithAllAppends();
            if (episode is null) return;

            List<Image> images = episode.TmdbEpisodeImages.Stills.ToList()
                .ConvertAll<Image>(image => new Image(image, episode, "still"));

            await mediaContext.Images.UpsertRange(images)
                .On(v => new { v.FilePath, v.EpisodeId })
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
                    EpisodeId = ti.EpisodeId
                })
                .RunAsync();

            ColorPaletteJob colorPaletteJob = new(episode.Id, "episode");
            JobDispatcher.Dispatch(colorPaletteJob, "image", 2);

            Logger.MovieDb(
                $@"TvShow {tv.Title}, Season {episode.SeasonNumber}, Episode {episode.EpisodeNumber}: Images stored");
            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private Task StoreTranslations()
    {
        foreach (var episode in _episodeAppends)
            try
            {
                Translation[] translations = episode.Translations.Translations.ToList()
                    .ConvertAll<Translation>(x => new Translation(x, episode)).ToArray();

                using MediaContext mediaContext = new();
                mediaContext.Translations
                    .UpsertRange(translations.Where(translation =>
                        translation.Title != null || translation.Overview != ""))
                    .On(t => new { t.Iso31661, t.Iso6391, t.EpisodeId })
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

                Logger.MovieDb(
                    $@"TvShow {show?.Name}, Season {tmdbSeason.SeasonNumber}, Episode {episode.EpisodeNumber}: Translations stored");
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _episodeAppends.Clear();
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public ValueTask DisposeAsync()
    {
        _episodeAppends.Clear();
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
        
        return new ValueTask(Task.CompletedTask);
    }
}