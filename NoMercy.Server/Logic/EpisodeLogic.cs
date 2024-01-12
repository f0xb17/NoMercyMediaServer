using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;

using Episode = NoMercy.Database.Models.Episode;

namespace NoMercy.Server.Logic;

public static class EpisodeLogic
{
    public static async Task FetchEpisodes(TvShowAppends show, SeasonAppends season)
    {        
        List<EpisodeAppends> episodes = [];
        Parallel.ForEach(season.Episodes, (episode, _) =>
        {
            try
            {
                EpisodeClient episodeClient = new EpisodeClient(show!.Id, season.SeasonNumber, episode.EpisodeNumber);
                episodes.Add(episodeClient.WithAllAppends().Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
        
        await Store(show, season, episodes);
        
    }
    
    private static async Task Store(TvShow show, Season season, List<EpisodeAppends> episodeAppends)
    {        
        Episode[] episodes = episodeAppends.ConvertAll<Episode>(x => new Episode(x, show.Id, season.Id)).ToArray();
        
        await MediaContext.Db.Episodes.UpsertRange(episodes)
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
            })
            .RunAsync();
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
    //         Console.WriteLine(e);
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
    //         Console.WriteLine(e);
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
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }
    
}