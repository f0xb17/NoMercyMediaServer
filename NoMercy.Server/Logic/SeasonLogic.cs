using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Jobs;
using Season = NoMercy.Database.Models.Season;

namespace NoMercy.Server.Logic;

public class SeasonLogic(TvShowAppends show)
{
    private readonly List<SeasonAppends> _seasonAppends = [];

    public async Task FetchSeasons()
    {
        Parallel.ForEach(show.Seasons, (season, _) =>
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
                Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
            }
        });

        await Store();

        lock (_seasonAppends)
        {
            Parallel.ForEach(_seasonAppends, (season, _) =>
            {
                try
                {
                    EpisodeLogic episodeLogic = new(show, season);
                    episodeLogic.FetchEpisodes().Wait();
                    episodeLogic.Dispose();
                }
                catch (Exception e)
                {
                    Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
                }
            });
        }

        await DispatchJobs();
    }

    private Task DispatchJobs()
    {
        lock (_seasonAppends)
        {
            foreach (var season in _seasonAppends ?? [])
            {
                ColorPaletteJob colorPaletteJob = new ColorPaletteJob(season.Id, "season");
                JobDispatcher.Dispatch(colorPaletteJob, "data");
            }
        }

        return Task.CompletedTask;
    }

    private Task Store()
    {
        lock (_seasonAppends)
        {
            Season[] seasons = _seasonAppends
                ?.ConvertAll<Season>(x => new Season(x, show.Id))
                .ToArray() ?? [];

            using MediaContext mediaContext = new MediaContext();
            mediaContext.Seasons.UpsertRange(seasons)
                .On(s => new { s.Id })
                .WhenMatched((ss, si) => new Season()
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
            
            Logger.MovieDb($@"TvShow {show.Name} Seasons stored");
            
        }
        
        return Task.CompletedTask;
    }
    
    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        var season = await mediaContext.Seasons
            .FirstOrDefaultAsync(e => e.Id == id);

        if (season is { _colorPalette: "", Poster: not null })
        {
            var palette = await ImageLogic.GenerateColorPalette(posterPath: season.Poster);
            season._colorPalette = palette;
            await mediaContext.SaveChangesAsync();
        }
    }
    
    // private async Task FetchCast()
    // {
    //     try
    //     {
    //         var cast = Show!.Credits!.Cast.ToList()
    //             .ConvertAll<Cast>(x => new Cast(x, Show!)).ToArray();
    //
    //         await MediaContext.Db.Casts
    //             .UpsertRange(cast)
    //             .On(c => new { c.Id })
    //             .WhenMatched((cs, ci) => new Cast()
    //             {
    //                 Id = ci.Id,
    //                 PersonId = ci.PersonId,
    //                 MovieId = ci.MovieId,
    //                 TvId = ci.TvId,
    //                 SeasonId = ci.SeasonId,
    //                 EpisodeId = ci.EpisodeId,
    //             })
    //             .RunAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
    //         throw;
    //     }
    // }
    //
    // private async Task FetchCrew()
    // {
    //     try
    //     {
    //         var crew = Show!.Credits!.Crew.ToList()
    //             .ConvertAll<Crew>(x => new Crew(x, Show!)).ToArray();
    //     
    //         await MediaContext.Db.Crews
    //             .UpsertRange(crew)
    //             .On(c => new { c.Id })
    //             .WhenMatched((cs, ci) => new Crew()
    //             {
    //                 Id = ci.Id,
    //                 PersonId = ci.PersonId,
    //                 MovieId = ci.MovieId,
    //                 TvId = ci.TvId,
    //                 SeasonId = ci.SeasonId,
    //                 EpisodeId = ci.EpisodeId,
    //             })
    //             .RunAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
    //         throw;
    //     }
    // }

    // private async Task FetchTranslations()
    // {
    //     try
    //     {
    //         var translations = Show!.Translations!.Translations.ToList()
    //             .ConvertAll<Translation>(x => new Translation(x, Show!)).ToArray();
    //     
    //         await MediaContext.Db.Translations
    //             .UpsertRange(translations)
    //             .On(t => new { t.Iso31661, t.Iso6391, t.TvId })
    //             .WhenMatched((ts, ti) => new Translation()
    //             {
    //                 Iso31661 = ti.Iso31661,
    //                 Iso6391 = ti.Iso6391,
    //                 Name = ti.Name,
    //                 EnglishName = ti.EnglishName,
    //                 Title = ti.Title,
    //                 Overview = ti.Overview,
    //                 Homepage = ti.Homepage,
    //                 Biography = ti.Biography,
    //                 TvId = ti.TvId,
    //                 SeasonId = ti.SeasonId,
    //                 EpisodeId = ti.EpisodeId,
    //                 MovieId = ti.MovieId,
    //                 CollectionId = ti.CollectionId,
    //                 PersonId = ti.PersonId,
    //             })
    //             .RunAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
    //         throw;
    //     }
    // }
    
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