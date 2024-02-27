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

public class EpisodeLogic(TvShowAppends show, SeasonAppends season, MediaContext mediaContext)
{
    private readonly List<EpisodeAppends?> _episodeAppends = [];
    
    public async Task FetchEpisodes()
    { 
        await Parallel.ForEachAsync(season.Episodes, (episode, _) =>
        {
            try
            {
                using EpisodeClient episodeClient = new(show.Id, season.SeasonNumber, episode.EpisodeNumber);
                using Task<EpisodeAppends?> episodeTask = episodeClient.WithAllAppends();
                lock (_episodeAppends)
                {
                    _episodeAppends.Add(episodeTask.Result);
                }
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
            
            return default;
        });
        
        await Store();

        // await StoreTranslations();

        await DispatchJobs();
    }

    private Task Store()
    {
        lock (_episodeAppends)
        {
            Episode[] episodes = _episodeAppends
                .ConvertAll<Episode>(x => new Episode(x, show.Id, season.Id))
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
                    _colorPalette = ei._colorPalette,
                })
                .Run();

            Logger.MovieDb($@"TvShow {show.Name}: Season {season.SeasonNumber}: Stored {season.Episodes.Length} episode{(season.Episodes.Length > 1 ? "s" : "")}"); 
            
        }
        
        return Task.CompletedTask;
    }
    
    private Task DispatchJobs()
    {
        lock (_episodeAppends)
        {
            foreach (var episode in _episodeAppends)
            {
                if (episode is null) continue;
                
                ColorPaletteJob colorPaletteJob = new ColorPaletteJob(id:episode.Id, model:"episode");
                JobDispatcher.Dispatch(colorPaletteJob, "data");
                    
                ImagesJob imagesJob = new ImagesJob(id:episode.Id, type:"episode", seasonNumber:season.SeasonNumber, episodeNumber:episode.EpisodeNumber);
                JobDispatcher.Dispatch(imagesJob, "queue", 2);
            }
        }
        
        return Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();
        var episode = await mediaContext.Episodes
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if(episode is null) return;

        lock (episode)
        {
            if (episode is { _colorPalette: "", Still: not null })
            {
                var palette = ImageLogic.GenerateColorPalette(stillPath: episode.Still).Result;
                episode._colorPalette = palette;
                mediaContext.SaveChanges();
            }
            
        }
    }

    public static async Task StoreImages(int show, int season, int id)
    {
        await using MediaContext mediaContext = new MediaContext();
        
        Tv? Show = await mediaContext.Tvs
            .FirstOrDefaultAsync(t => t.Id == show);
        
        EpisodeClient episodeClient = new(show, season, id);
        EpisodeAppends? episode = episodeClient.WithAllAppends().Result;
        
        if (episode is null) return;
        
        var images = episode.Images.Stills.ToList()
            .ConvertAll<Image>(image => new Image(image:image, episode:episode, type:"still")) ?? [];
        
        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.EpisodeId })
            .WhenMatched((ts, ti) => new Image
            {
                AspectRatio = ti.AspectRatio,
                FilePath = ti.FilePath,
                Height = ti.Height,
                Iso6391 = ti.Iso6391,
                VoteAverage = ti.VoteAverage,
                VoteCount = ti.VoteCount,
                Width = ti.Width,
                Type = ti.Type,
                EpisodeId = ti.EpisodeId,
            })
            .RunAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            ColorPaletteJob colorPaletteJob = new ColorPaletteJob(filePath:image.FilePath, model:"image");
            JobDispatcher.Dispatch(colorPaletteJob, "data");
        }

        Logger.MovieDb($@"TvShow {Show.Title}, Season {episode.SeasonNumber}, Episode {episode.EpisodeNumber}: Images stored");
        await Task.CompletedTask;
    }

    private Task StoreTranslations()
    {
        lock (_episodeAppends)
        {
            foreach (var episode in _episodeAppends)
            {
                if (episode is null) continue;

                try
                {
                    var translations = episode.Translations.Translations.ToList()
                        .ConvertAll<Translation>(x => new Translation(x, episode)).ToArray() ?? [];

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
                    throw;
                }
            }
        }
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        lock (_episodeAppends)
        {
            _episodeAppends.Clear();
        }
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}