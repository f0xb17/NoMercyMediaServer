using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

public class PersonJob : IShouldQueue
{
    private readonly int? _id;
    private readonly string? _type;
    
    public PersonJob(long id, string type)
    {
        _id = (int)id;
        _type = type;
    }
    
    public async Task Handle()
    {
        switch (_type)
        {
            case "tv" when _id != null:
            {
                TvShowAppends? show = await new TvClient(_id).WithAllAppends();
            
                PersonLogic personShowLogic = new PersonLogic(show);
                await personShowLogic.FetchPeople();
                personShowLogic.Dispose();
            
                foreach (var s in show.Seasons)
                {
                    using SeasonClient seasonClient = new(show.Id, s.SeasonNumber);
                    SeasonAppends? season = await seasonClient.WithAllAppends();
                
                    PersonLogic personSeasonLogic = new PersonLogic(show, season);
                    await personSeasonLogic.FetchPeople();
                    personSeasonLogic.Dispose();
                
                    foreach (var e in season.Episodes)
                    {
                        using EpisodeClient episodeClient = new(show.Id, season.SeasonNumber, e.EpisodeNumber);
                        EpisodeAppends? episode = await episodeClient.WithAllAppends();

                        PersonLogic personEpisodeLogic = new PersonLogic(show, season, episode);
                        await personEpisodeLogic.FetchPeople();
                        personEpisodeLogic.Dispose();
                    }
                }

                break;
            }
            
            case "movie" when _id != null:
            {
                MovieAppends? movie = await new MovieClient(_id).WithAllAppends();
                
                PersonLogic personMovieLogic = new PersonLogic(movie);
                await personMovieLogic.FetchPeople();
                personMovieLogic.Dispose();
                break;
            }
            
            default:
                throw new Exception("Invalid model Type");
        }
    }
}