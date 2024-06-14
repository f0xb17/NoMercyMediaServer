using System.Collections.Concurrent;
using FlexLabs.EntityFrameworkCore.Upsert;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using Person = NoMercy.Database.Models.Person;
using Cast = NoMercy.Database.Models.Cast;
using Crew = NoMercy.Database.Models.Crew;
using Role = NoMercy.Database.Models.Role;
using Job = NoMercy.Database.Models.Job;
using Exception = System.Exception;
using GuestStar = NoMercy.Database.Models.GuestStar;
using Image = NoMercy.Database.Models.Image;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public class PersonLogic : IDisposable, IAsyncDisposable
{
    private enum Type
    {
        TvShow,
        Season,
        Episode,
        Movie
    }

    private readonly TmdbTvShowAppends? _show;
    private readonly TmdbSeasonAppends? _season;
    private readonly TmdbEpisodeAppends? _episode;
    private readonly TmdbMovieAppends? _movie;
    private readonly string? _name;

    private readonly string? _logPrefix;
    private readonly Type _currentType;

    public PersonLogic(TmdbTvShowAppends? show)
    {
        if (show is null) return;

        _show = show;
        _logPrefix = $"TvShow {show.Name}:";
        _currentType = Type.TvShow;
        _name = show.Name;

        _season = new TmdbSeasonAppends();
        _episode = new TmdbEpisodeAppends();
        _movie = new TmdbMovieAppends();
    }

    public PersonLogic(TmdbTvShowAppends? show, TmdbSeasonAppends? season)
    {
        if (show is null || season is null) return;

        _show = show;
        _season = season;
        _logPrefix = $"TvShow {show.Name}, Season {season.SeasonNumber}:";
        _currentType = Type.Season;
        _name = _season.Name;

        _episode = new TmdbEpisodeAppends();
        _movie = new TmdbMovieAppends();
    }

    public PersonLogic(TmdbTvShowAppends? show, TmdbSeasonAppends? season, TmdbEpisodeAppends? episode)
    {
        if (show is null || season is null || episode is null) return;

        _show = show;
        _season = season;
        _episode = episode;
        _logPrefix = $"TvShow {show.Name}, Season {season.SeasonNumber}, Episode {episode.EpisodeNumber}:";
        _currentType = Type.Episode;
        _name = _episode.Name;

        _movie = new TmdbMovieAppends();
    }

    public PersonLogic(TmdbMovieAppends? movie)
    {
        if (movie is null) return;

        _movie = movie;
        _logPrefix = $"Movie {movie.Title}:";
        _currentType = Type.Movie;
        _name = "";

        _show = new TmdbTvShowAppends();
        _season = new TmdbSeasonAppends();
        _episode = new TmdbEpisodeAppends();
    }

    private readonly ConcurrentStack<TmdbPersonAppends?> _personAppends = [];

    private async Task FetchPeopleByCast(TmdbCast[] cast)
    {
        await Parallel.ForEachAsync(cast, async (person, _) =>
        {
            try
            {
                await using var personClient = new TmdbPersonClient(person.Id);
                var personTask = await personClient.WithAllAppends();
                _personAppends.Push(personTask);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });
    }

    private async Task FetchPeopleByGuestStars(TmdbGuestStar[] cast)
    {
        await Parallel.ForEachAsync(cast, async (person, _) =>
        {
            try
            {
                await using var personClient = new TmdbPersonClient(person.Id);
                var personTask = await personClient.WithAllAppends();
                _personAppends.Push(personTask);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });
    }

    private async Task FetchPeopleByCrew(TmdbCrew[] crew)
    {
        await Parallel.ForEachAsync(crew, async (person, _) =>
        {
            try
            {
                await using TmdbPersonClient tmdbPersonClient = new(person.Id);
                var personTask = await tmdbPersonClient.WithAllAppends();
                _personAppends.Push(personTask);

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });
    }

    public async Task FetchPeople()
    {
        switch (_currentType)
        {
            case Type.TvShow:
                if (_show is null) return;

                await FetchPeopleByCast(_show.Credits.Cast);
                await FetchPeopleByCrew(_show.Credits.Crew);

                await Store();

                await StoreCast(_show.Credits.Cast, _show);
                await StoreCreator(_show.CreatedBy, _show);
                await StoreCrew(_show.Credits.Crew, _show);

                //:TODO: Fix this
                // await StoreAggregateCredits(_show.AggregateCredits, _show);
                break;
            case Type.Season:
                if (_season is null) return;

                await FetchPeopleByCast(_season.TmdbSeasonCredits.Cast);
                await FetchPeopleByCrew(_season.TmdbSeasonCredits.Crew);

                await Store();

                await StoreCast(_season.TmdbSeasonCredits.Cast, _season);
                await StoreCrew(_season.TmdbSeasonCredits.Crew, _season);

                await StoreAggregateCredits(_season.AggregateCredits, _season);
                break;
            case Type.Episode:
                if (_episode is null) return;

                await FetchPeopleByCast(_episode.Cast);
                await FetchPeopleByCrew(_episode.Crew);
                await FetchPeopleByGuestStars(_episode.GuestStars);

                await Store();

                await StoreCast(_episode.TmdbEpisodeCredits.Cast, _episode);
                await StoreCrew(_episode.TmdbEpisodeCredits.Crew, _episode);

                await StoreGuestStars(_episode.GuestStars, _episode);
                break;
            case Type.Movie:
                if (_movie is null) return;

                await FetchPeopleByCast(_movie.Credits.Cast);
                await FetchPeopleByCrew(_movie.Credits.Crew);

                await Store();

                await StoreCast(_movie.Credits.Cast, _movie);
                await StoreCrew(_movie.Credits.Crew, _movie);

                break;

            default:
                throw new Exception("Invalid model Type");
        }

        DispatchJobs();
    }

    private async Task Store()
    {
        try
        {
            var people = _personAppends.ToList()
                .ConvertAll<Person>(x => new Person(x)).ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.People.UpsertRange(people)
                .On(p => new { p.Id })
                .WhenMatched((ps, pi) => new Person
                {
                    Id = pi.Id,
                    Adult = pi.Adult,
                    AlsoKnownAs = pi.AlsoKnownAs,
                    Biography = pi.Biography,
                    BirthDay = pi.BirthDay,
                    DeathDay = pi.DeathDay,
                    _externalIds = pi._externalIds,
                    TmdbGender = pi.TmdbGender,
                    Homepage = pi.Homepage,
                    ImdbId = pi.ImdbId,
                    KnownForDepartment = pi.KnownForDepartment,
                    Name = pi.Name,
                    PlaceOfBirth = pi.PlaceOfBirth,
                    Popularity = pi.Popularity,
                    Profile = pi.Profile,
                    TitleSort = pi.Name
                })
                .RunAsync();

            await StoreTranslations(_personAppends);

            Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto
            {
                QueryKey = ["person"]
            });

            Logger.MovieDb($@"{_logPrefix} {_name} stored {people.Length} people");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private static Role[] Roles(Role[]? roles)
    {
        if (roles is null || roles.Length == 0) return Array.Empty<Role>();

        using var mediaContext = new MediaContext();
        return mediaContext.Roles
            .Where(role => roles.Select(r => r.CreditId).Contains(role.CreditId))
            .ToArray();
    }

    private async Task StoreCast(TmdbCast[] casts, dynamic? model)
    {
        try
        {
            var roles = casts
                .ToList()
                .ConvertAll<Role>(x => new Role(x))
                .Where(cast => cast.CreditId is not null)
                .ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.Roles
                .UpsertRange(roles)
                .On(p => new { p.CreditId })
                .WhenMatched((rs, ri) => new Role
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    Order = ri.Order,
                    CreditId = ri.CreditId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Roles stored");

            Role[] crewArray = Roles(roles);

            Cast[] cast = casts
                .Where(cast => cast.CreditId is not "" && cast.CreditId is not null)
                .ToList()
                .ConvertAll<Cast>(x => new Cast(x, model, _movie, _show, _season, crewArray))
                .ToArray();

            UpsertCommandBuilder<Cast> query = _currentType switch
            {
                Type.Movie => mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.MovieId, c.RoleId }),
                Type.TvShow => mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.TvId, c.RoleId }),
                Type.Season => mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.SeasonId, c.RoleId }),
                Type.Episode => mediaContext.Casts.UpsertRange(cast)
                    .On(c => new { c.CreditId, c.EpisodeId, c.RoleId }),
                _ => throw new ArgumentOutOfRangeException()
            };

            await query.WhenMatched((cs, ci) => new Cast
                {
                    CreditId = ci.CreditId,
                    MovieId = ci.MovieId,
                    TvId = ci.TvId,
                    SeasonId = ci.SeasonId,
                    EpisodeId = ci.EpisodeId,
                    PersonId = ci.PersonId,
                    RoleId = ci.RoleId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Cast stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private static Job[] Jobs(Job[]? jobs)
    {
        if (jobs is null || jobs.Length == 0) return Array.Empty<Job>();

        using var mediaContext = new MediaContext();
        return mediaContext.Jobs
            .Where(job => jobs.Select(j => j.CreditId).Contains(job.CreditId))
            .ToArray();
    }

    private async Task StoreCrew(TmdbCrew[] crews, dynamic? model)
    {
        try
        {
            var jobs = crews
                .ToList()
                .ConvertAll<Job>(x => new Job(x))
                .Where(crew => crew.CreditId is not null)
                .ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.Jobs.UpsertRange(jobs)
                .On(p => new { p.CreditId })
                .WhenMatched((js, ji) => new Job
                {
                    Task = ji.Task,
                    CreditId = ji.CreditId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Jobs stored");

            Job[] jobArray = Jobs(jobs);

            Crew[] crew = crews
                .Where(crew => crew.CreditId is not "" && crew.CreditId is not null)
                .ToList()
                .ConvertAll<Crew>(x => new Crew(x, model, _movie, _show, _season, jobArray))
                .ToArray();

            UpsertCommandBuilder<Crew> c1 = _currentType switch
            {
                Type.Movie => mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.MovieId, c.JobId }),
                Type.TvShow => mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.TvId, c.JobId }),
                Type.Season => mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.SeasonId, c.JobId }),
                Type.Episode => mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.EpisodeId, c.JobId }),
                _ => throw new ArgumentOutOfRangeException()
            };

            await c1.WhenMatched((cs, ci) => new Crew
                {
                    CreditId = ci.CreditId,
                    MovieId = ci.MovieId,
                    TvId = ci.TvId,
                    SeasonId = ci.SeasonId,
                    EpisodeId = ci.EpisodeId,
                    PersonId = ci.PersonId,
                    JobId = ci.JobId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Crew stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreCreator(TmdbCreatedBy[] showCreatedBy, TmdbTvShowAppends show)
    {
        try
        {
            var creators = showCreatedBy
                .ToList()
                .ConvertAll<Creator>(x => new Creator(x, show))
                .ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.Creators.UpsertRange(creators)
                .On(c => new { c.TvId, c.PersonId })
                .WhenMatched((cs, ci) => new Creator
                {
                    TvId = ci.TvId,
                    PersonId = ci.PersonId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Creator stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreGuestStars(TmdbGuestStar[] guests, TmdbEpisodeAppends? episode)
    {
        try
        {
            var guestStars = guests
                .ToList()
                .ConvertAll<GuestStar>(x => new GuestStar(x, episode))
                .Where(crew => crew.CreditId is not "")
                .ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.GuestStars
                .UpsertRange(guestStars)
                .On(c => new { c.CreditId, c.EpisodeId })
                .WhenMatched((cs, ci) => new GuestStar
                {
                    Id = ci.Id,
                    CreditId = ci.CreditId,
                    PersonId = ci.PersonId,
                    EpisodeId = ci.EpisodeId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} Cast stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        try
        {
            Role[] roles = guests.ToList()
                .ConvertAll<Role>(x => new Role(x))
                .ToArray();

            await using var mediaContext = new MediaContext();
            await mediaContext.Roles
                .UpsertRange(roles.Where(role => role.Character is not null && role.CreditId is not null))
                .On(p => new { p.GuestStarId })
                .WhenMatched((rs, ri) => new Role
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    CreditId = ri.CreditId,
                    GuestStarId = ri.GuestStarId
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} Roles stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreAggregateCredits(TmdbTvAggregatedCredits showAggregateCredits, TmdbTvShowAppends? show)
    {
        if (show is null) return;

        try
        {
            await StoreAggregateCast(showAggregateCredits.Cast, show);
            await StoreAggregateCrew(showAggregateCredits.Crew, show);

            Logger.MovieDb($@"{_logPrefix} {show.Name}: AggregateCredits stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreAggregateCredits(TmdbSeasonAggregatedCredits tmdbSeasonAggregateCredits, TmdbSeasonAppends? season)
    {
        if (_show is null || season is null) return;
        try
        {
            await StoreAggregateCast(tmdbSeasonAggregateCredits.Cast, season);
            await StoreAggregateCrew(tmdbSeasonAggregateCredits.Crew, season);

            Logger.MovieDb($@"{_logPrefix} TvShow {_show.Name}, Season {season.SeasonNumber}: AggregateCredits stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreAggregateCast(TmdbTmdbAggregatedCast[] aggregatedCasts, dynamic? model)
    {
        List<TmdbCast> cast = [];

        cast.AddRange(aggregatedCasts.Select(aggregatedCast => new TmdbCast
        {
            Adult = aggregatedCast.Adult,
            Gender = aggregatedCast.Gender,
            Id = aggregatedCast.Id,
            KnownForDepartment = aggregatedCast.KnownForDepartment,
            Name = aggregatedCast.Name,
            OriginalName = aggregatedCast.OriginalName,
            Popularity = aggregatedCast.Popularity,
            ProfilePath = aggregatedCast.ProfilePath,
            Order = aggregatedCast.Order
        }));

        await StoreCast(cast.ToArray(), model);

        foreach (var aggregatedCast in aggregatedCasts) await StoreRoles(aggregatedCast.Roles);
    }

    private async Task StoreAggregateCrew(TmdbTmdbAggregatedCrew[] aggregatedCrews, dynamic? model)
    {
        List<TmdbCrew> crew = [];

        crew.AddRange(aggregatedCrews.Select(aggregatedCrew => new TmdbCrew
        {
            Department = aggregatedCrew.KnownForDepartment,
            Adult = aggregatedCrew.Adult,
            Gender = aggregatedCrew.Gender,
            Id = aggregatedCrew.Id,
            KnownForDepartment = aggregatedCrew.KnownForDepartment,
            Name = aggregatedCrew.Name,
            OriginalName = aggregatedCrew.OriginalName,
            Popularity = aggregatedCrew.Popularity,
            ProfilePath = aggregatedCrew.ProfilePath,
            Order = aggregatedCrew.Order
        }));

        await StoreCrew(crew.ToArray(), model);

        foreach (var aggregatedCrew in aggregatedCrews) await StoreJobs(aggregatedCrew.Jobs);
    }

    private async Task StoreRoles(TmdbAggregatedCreditRole[] role)
    {
        var roles = role.ToList()
            .ConvertAll<Role>(x => new Role(x)).ToArray();

        try
        {
            await using var mediaContext = new MediaContext();
            await mediaContext.Roles
                .UpsertRange(roles.Where(r => r.Character is not null && r.CreditId is not null))
                .On(p => new { p.CreditId })
                .WhenMatched((rs, ri) => new Role()
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    CreditId = ri.CreditId
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreJobs(TmdbAggregatedCrewJob[] job)
    {
        var jobs = job.ToList()
            .ConvertAll<Job>(x => new Job(x)).ToArray();

        try
        {
            await using var mediaContext = new MediaContext();
            await mediaContext.Jobs.UpsertRange(jobs)
                .On(p => new { p.CreditId })
                .WhenMatched((js, ji) => new Job()
                {
                    Task = ji.Task,
                    EpisodeCount = ji.EpisodeCount,
                    CreditId = ji.CreditId
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private void DispatchJobs()
    {
        foreach (var person in _personAppends)
        {
            if (person is null) continue;

            var personImagesJob = new PersonImagesJob(person.Id);
            JobDispatcher.Dispatch(personImagesJob, "data", 2);
        }
    }

    public static async Task StoreImages(int id)
    {
        TmdbPersonClient tmdbPersonClient = new(id);
        var personAppend = await tmdbPersonClient.WithAllAppends();

        if (personAppend is null) return;

        var images = personAppend.Images.Profiles.ToList()
            .ConvertAll<Image>(profile => new Image(profile, personAppend, "profile"));

        await using MediaContext mediaContext = new();
        await mediaContext.Images.UpsertRange(images)
            .On(v => new { v.FilePath, v.PersonId })
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
                PersonId = ti.PersonId
            })
            .RunAsync();

        var personColorPaletteJob = new ColorPaletteJob(id: personAppend.Id, model: "person");
        JobDispatcher.Dispatch(personColorPaletteJob, "image", 2);

        Logger.MovieDb($@"Person {personAppend.Name}: Images stored");
        await Task.CompletedTask;
    }

    public static async Task Palette(int id)
    {
        await using var mediaContext = new MediaContext();
        Logger.Queue($"Fetching color palette for Movie {id}");

        var person = await mediaContext.People
            .Where(e => e._colorPalette == "")
            .FirstOrDefaultAsync(e => e.Id == id);

        if (person is { _colorPalette: "" })
        {
            person._colorPalette = await MovieDbImage.ColorPalette("profile", person.Profile);

            await mediaContext.SaveChangesAsync();
        }

        var images = await mediaContext.Images
            .Where(e => e.MovieId == id)
            .Where(e => e._colorPalette == "")
            .ToArrayAsync();

        foreach (var image in images)
        {
            if (string.IsNullOrEmpty(image.FilePath)) continue;

            image._colorPalette = await MovieDbImage.ColorPalette("image", image.FilePath);
        }
    }

    private async Task StoreTranslations(ConcurrentStack<TmdbPersonAppends?> personAppends)
    {
        try
        {
            ConcurrentStack<Translation> translations = [];

            Parallel.ForEach(personAppends, person =>
            {
                if (person is null) return;

                translations.PushRange(person.Translations.Translations.ToList()
                    .ConvertAll<Translation>(x => new Translation(x, person)).ToArray());
            });

            await using var mediaContext = new MediaContext();
            await mediaContext.Translations
                .UpsertRange(translations.Where(translation => translation.Title != null || translation.Overview != ""))
                .On(t => new { t.Iso31661, t.Iso6391, t.PersonId })
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
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
            throw;
        }
    }

    public void Dispose()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public async ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }
}