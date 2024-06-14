using NoMercy.Providers.TMDB.Client;
using NoMercy.Server.app.Helper;
using NoMercy.Server.Logic;
using NoMercy.Server.system;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class PersonJob : IShouldQueue
{
    public int? Id { get; set; }
    public string? Type { get; set; }

    public PersonJob()
    {
        //
    }

    public PersonJob(int id, string type)
    {
        Id = id;
        Type = type;
    }

    public async Task Handle()
    {
        switch (Type)
        {
            case "tv" when Id != null:
            {
                var show = await new TmdbTvClient(Id).WithAllAppends();
                if (show is null) return;

                await using var personShowLogic = new PersonLogic(show);
                await personShowLogic.FetchPeople();

                foreach (var s in show.Seasons)
                {
                    await using TmdbSeasonClient tmdbSeasonClient = new(show.Id, s.SeasonNumber);
                    var season = await tmdbSeasonClient.WithAllAppends();
                    if (season is null) continue;

                    await using var personSeasonLogic = new PersonLogic(show, season);
                    await personSeasonLogic.FetchPeople();

                    foreach (var e in season.Episodes)
                    {
                        await using TmdbEpisodeClient tmdbEpisodeClient = new(show.Id, season.SeasonNumber, e.EpisodeNumber);
                        var episode = await tmdbEpisodeClient.WithAllAppends();

                        await using var personEpisodeLogic = new PersonLogic(show, season, episode);
                        await personEpisodeLogic.FetchPeople();
                    }
                }

                Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
                {
                    QueryKey = ["tv", Id]
                });


                break;
            }

            case "movie" when Id != null:
            {
                var movie = await new TmdbMovieClient(Id).WithAllAppends();

                await using var personMovieLogic = new PersonLogic(movie);
                await personMovieLogic.FetchPeople();

                Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
                {
                    QueryKey = ["movie", Id]
                });
                break;
            }

            default:
                throw new Exception("Invalid model Type");
        }
    }
}