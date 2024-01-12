using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;

using Season = NoMercy.Database.Models.Season;

namespace NoMercy.Server.Logic;

public static class SeasonLogic
{    
    public static async Task FetchSeasons(TvShowAppends show)
    {   
        List<SeasonAppends> seasons = [];

        Parallel.ForEach(show!.Seasons, (season, _) =>
        {
            try
            {
                SeasonClient seasonClient = new SeasonClient(show.Id, season.SeasonNumber);
                SeasonAppends seasonAppends = seasonClient.WithAllAppends().Result;
                seasons.Add(seasonAppends);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        });
        
        await Store(show, seasons);
        
        foreach (var season in seasons)
        {
            await EpisodeLogic.FetchEpisodes(show, season);
        }
    }

    private static async Task Store(TvShow show, List<SeasonAppends> seasonResponse)
    {
        Season[] seasons = seasonResponse.ConvertAll<Season>(x => new Season(x, show.Id)).ToArray();

        await MediaContext.Db.Seasons.UpsertRange(seasons)
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