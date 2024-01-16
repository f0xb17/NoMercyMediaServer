using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Jobs;
using Episode = NoMercy.Database.Models.Episode;

namespace NoMercy.Server.Logic;

public class EpisodeLogic(TvShowAppends show, SeasonAppends season)
{
    private readonly List<EpisodeAppends> _episodeAppends = [];  
    
    public async Task FetchEpisodes()
    { 
        await Parallel.ForEachAsync(season.Episodes, (episode, _) =>
        {
            try
            {
                using EpisodeClient episodeClient = new(show.Id, season.SeasonNumber, episode.EpisodeNumber);
                using Task<EpisodeAppends> episodeTask = episodeClient.WithAllAppends();
                lock (_episodeAppends)
                {            
                    _episodeAppends.Add(episodeTask.Result);
                }
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
            }

            return default;
        });
        
        await Store();

        await DispatchJobs();
    }

    private Task DispatchJobs()
    {
        lock (_episodeAppends)
        {
            foreach (var episode in _episodeAppends ?? [])
            {
                ColorPaletteJob colorPaletteJob = new ColorPaletteJob(episode.Id, "episode");
                JobDispatcher.Dispatch(colorPaletteJob, "data");
            }
        }
        
        return Task.CompletedTask;
    }

    private Task Store()
    {
        lock (_episodeAppends)
        {
            Episode[] episodes = _episodeAppends
                ?.ConvertAll<Episode>(x => new Episode(x, show.Id, season.Id))
                .ToArray() ?? [];
            
            using MediaContext mediaContext = new MediaContext();
            mediaContext.Episodes.UpsertRange(episodes)
                .On(e => new { e.Id })
                .WhenMatched((es, ei) => new Episode()
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

            Logger.MovieDb($@"TvShow {show.Name} Season {season.SeasonNumber} stored {season.Episodes.Length} episode{(season.Episodes.Length > 1 ? "s" : "")}"); 
            
        }
        
        return Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();
        var episode = await mediaContext.Episodes
            .FirstOrDefaultAsync(e => e.Id == id);
        
        if (episode is { _colorPalette: "", Still: not null })
        {
            var palette = await ImageLogic.GenerateColorPalette(stillPath: episode.Still);
            episode._colorPalette = palette;
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
        lock (_episodeAppends)
        {
            _episodeAppends.Clear();
        }
        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}