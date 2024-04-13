using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using Episode = NoMercy.Database.Models.Episode;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public class EpisodeLogic(TvShow show, SeasonDetails season)
{
    private readonly ConcurrentStack<EpisodeAppends> _episodeAppends = [];

    public async Task FetchEpisodes()
    {
        await Parallel.ForEachAsync(season.Episodes, async (episode, _) =>
        {
            try
            {
                using EpisodeClient episodeClient = new(show.Id, episode.SeasonNumber, episode.EpisodeNumber);
                EpisodeAppends? episodeTask = await episodeClient.WithAllAppends();
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

        await DispatchJobs();
    }

    private Task Store()
    {
        try
        {
            Episode[] episodes = _episodeAppends.ToList()
                .ConvertAll<Episode>(x => new Episode(x, show.Id, season.Id))
                .ToArray();

            using MediaContext mediaContext = new MediaContext();
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
                    _colorPalette = ei._colorPalette,
                })
                .Run();

            Logger.MovieDb(
                $@"TvShow {show.Name}: Season {season.SeasonNumber}: Stored {season.Episodes.Length} episode{(season.Episodes.Length > 1 ? "s" : "")}");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        return Task.CompletedTask;
    }

    private async Task DispatchJobs()
    {
        foreach (EpisodeAppends episode in _episodeAppends)
        {
            try
            {
                await new ColorPaletteJob(id: episode.Id, model: "episode").Handle();
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }

            try
            {
                EpisodeImagesJob imagesJob = new EpisodeImagesJob(id: episode.Id,
                    seasonNumber: episode.SeasonNumber, episodeNumber: episode.EpisodeNumber);
                JobDispatcher.Dispatch(imagesJob, "data", 2);
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }
    }

    public static async Task GetPalette(int id)
    {
        try
        {
            await using MediaContext mediaContext = new MediaContext();
            Episode? episode = await mediaContext.Episodes
                .FirstOrDefaultAsync(e => e.Id == id);

            if (episode is { _colorPalette: "", Still: not null })
            {
                string palette = await ImageLogic.GenerateColorPalette(stillPath: episode.Still);
                episode._colorPalette = palette;
                await mediaContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    public static async Task StoreImages(int show, int season, int episodeNumber)
    {
        try
        {
            await using MediaContext mediaContext = new MediaContext();

            Tv? tv = await mediaContext.Tvs
                .FirstOrDefaultAsync(t => t.Id == show);

            if (tv is null) return;

            EpisodeClient episodeClient = new(show, season, episodeNumber);
            EpisodeAppends? episode = await episodeClient.WithAllAppends();
            if (episode is null) return;

            List<Image> images = episode.Images.Stills.ToList()
                .ConvertAll<Image>(image => new Image(image: image, episode: episode, type: "still"));

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
                    EpisodeId = ti.EpisodeId,
                })
                .RunAsync();

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
        foreach (EpisodeAppends episode in _episodeAppends)
        {
            try
            {
                Translation[] translations = episode.Translations.Translations.ToList()
                    .ConvertAll<Translation>(x => new Translation(x, episode)).ToArray();

                using MediaContext mediaContext = new MediaContext();
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
                        PersonId = ti.PersonId,
                    })
                    .Run();

                Logger.MovieDb(
                    $@"TvShow {show?.Name}, Season {season.SeasonNumber}, Episode {episode.EpisodeNumber}: Translations stored");
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _episodeAppends.Clear();

        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}