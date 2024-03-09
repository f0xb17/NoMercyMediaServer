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

public class SeasonLogic(TvShowAppends show, MediaContext mediaContext)
{
    private readonly List<SeasonAppends?> _seasonAppends = [];

    public async Task FetchSeasons()
    {
        await Parallel.ForEachAsync(show.Seasons, (season, _) =>
        {
            try
            {
                using SeasonClient seasonClient = new(show.Id, season.SeasonNumber);
                using Task<SeasonAppends> seasonTask = seasonClient.WithAllAppends();
                lock (_seasonAppends)
                {
                    _seasonAppends.Add(seasonTask.Result);
                }
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }

            return default;
        });

        await Store();
        
        await StoreEpisodes();
        
        await StoreTranslations();
        
        await DispatchJobs();
    }

    private Task DispatchJobs()
    {
        lock (_seasonAppends)
        {
            foreach (var season in _seasonAppends)
            {
                if (season is null) continue;

                try
                {
                    ColorPaletteJob colorPaletteJob = new ColorPaletteJob(id: season.Id, model: "season");
                    JobDispatcher.Dispatch(colorPaletteJob, "data").Wait();
                }
                catch (Exception e)
                {
                    Logger.MovieDb(e, LogLevel.Error);
                }

                try
                {
                    ImagesJob imagesJob =
                        new ImagesJob(id: season.Id, type: "season", seasonNumber: season.SeasonNumber);
                    JobDispatcher.Dispatch(imagesJob, "queue", 2).Wait();
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

    private Task Store()
    {
        lock (_seasonAppends)
        {
            try
            {
                Season[] seasons = _seasonAppends
                    .ConvertAll<Season>(x => new Season(x, show.Id))
                    .ToArray();

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
                throw;
            }

            Logger.MovieDb($@"TvShow {show.Name}: Seasons stored");

        }

        return Task.CompletedTask;
    }
    
    private Task StoreEpisodes()
    {
        lock (_seasonAppends)
        {
            foreach (var season in _seasonAppends)
            {
                if (season is null) continue;

                try
                {
                    EpisodeLogic episodeLogic = new(show, season, mediaContext);
                    episodeLogic.FetchEpisodes().Wait();
                    episodeLogic.Dispose();
                }
                catch (Exception e)
                {
                    Logger.Encoder(e, LogLevel.Error);
                }
            }
        }

        return Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        var season = await mediaContext.Seasons
            .FirstOrDefaultAsync(e => e.Id == id);

        if (season is null) return;

        lock (season)
        {
            if (season is { _colorPalette: "", Poster: not null })
            {
                var palette = ImageLogic.GenerateColorPalette(posterPath: season.Poster).Result;
                season._colorPalette = palette;
                mediaContext.SaveChanges();
            }

        }
    }

    public static async Task StoreImages(int show, int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        Tv? Show = await mediaContext.Tvs
            .FirstOrDefaultAsync(tv => tv.Id == show);

        SeasonClient seasonClient = new(show, id);
        SeasonAppends season = seasonClient.WithAllAppends().Result;

        if (season is null) return;
        
        var images = season.Images.Posters.ToList()
            .ConvertAll<Image>(image => new Image(image: image, season: season, type: "poster")) ?? [];

        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.SeasonId })
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
                SeasonId = ti.SeasonId,
            })
            .RunAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;
            ColorPaletteJob colorPaletteJob = new ColorPaletteJob(filePath: image.FilePath, model: "image");
            JobDispatcher.Dispatch(colorPaletteJob, "data").Wait();
        }

        Logger.MovieDb($@"TvShow {Show?.Title}, Season {season.SeasonNumber}: Images stored");
        await Task.CompletedTask;
    }

    private Task StoreTranslations()
    {
        lock (_seasonAppends)
        {
            foreach (var season in _seasonAppends)
            {
                if (season is null) continue;

                var translations = season.Translations.Translations.ToList()
                    .ConvertAll<Translation>(x => new Translation(x, season)).ToArray() ?? [];

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
        }
        
        return Task.CompletedTask;
    }

public void Dispose()
    {
        lock (_seasonAppends)
        {
            _seasonAppends.Clear();
        }
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}