using FlexLabs.EntityFrameworkCore.Upsert;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Server.app.Jobs;
using NoMercy.Server.system;
using TMDBCast = NoMercy.Providers.TMDB.Models.Shared.Cast;
using TMDBCrew = NoMercy.Providers.TMDB.Models.Shared.Crew;
using TMDBGuestStar = NoMercy.Providers.TMDB.Models.Shared.GuestStar;
using PersonAppends = NoMercy.Providers.TMDB.Models.People.PersonAppends;
using TvShowAppends = NoMercy.Providers.TMDB.Models.TV.TvShowAppends;
using SeasonAppends = NoMercy.Providers.TMDB.Models.Season.SeasonAppends;
using EpisodeAppends = NoMercy.Providers.TMDB.Models.Episode.EpisodeAppends;
using MovieAppends = NoMercy.Providers.TMDB.Models.Movies.MovieAppends;
using TvAggregatedCredits = NoMercy.Providers.TMDB.Models.TV.TvAggregatedCredits;

using Person = NoMercy.Database.Models.Person;
using Cast = NoMercy.Database.Models.Cast;
using Crew = NoMercy.Database.Models.Crew;
using Role = NoMercy.Database.Models.Role;
using Job = NoMercy.Database.Models.Job;

using Exception = System.Exception;
using GuestStar = NoMercy.Database.Models.GuestStar;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Logic;

public class PersonLogic
{
    private enum Type
    {
        TvShow,
        Season,
        Episode,
        Movie
    }

    private readonly TvShowAppends? _show;
    private readonly SeasonAppends? _season;
    private readonly EpisodeAppends? _episode;
    private readonly MovieAppends? _movie;
    private readonly string? _name;

    private readonly string? _logPrefix;
    private readonly Type _currentType;
    private readonly MediaContext _mediaContext = new();

    public PersonLogic(TvShowAppends? show)
    {
        if (show is null) return;
        
        _show = show;
        _logPrefix = $"TvShow {show.Name}:";
        _currentType = Type.TvShow;
        _name = show.Name;

        _season = new SeasonAppends();
        _episode = new EpisodeAppends();
        _movie = new MovieAppends();
    }

    public PersonLogic(TvShowAppends? show, SeasonAppends? season)
    {
        if (show is null || season is null) return;
        
        _show = show;
        _season = season;
        _logPrefix = $"TvShow {show.Name}, Season {season.SeasonNumber}:";
        _currentType = Type.Season;
        _name = _season.Name;

        _episode = new EpisodeAppends();
        _movie = new MovieAppends();
    }

    public PersonLogic(TvShowAppends? show, SeasonAppends? season, EpisodeAppends? episode)
    {
        if (show is null || season is null || episode is null) return;
        
        _show = show;
        _season = season;
        _episode = episode;
        _logPrefix = $"TvShow {show.Name}, Season {season.SeasonNumber}, Episode {episode.EpisodeNumber}:";
        _currentType = Type.Episode;
        _name = _episode.Name;

        _movie = new MovieAppends();
    }

    public PersonLogic(MovieAppends? movie)
    {
        if (movie is null) return;
        
        _movie = movie;
        _logPrefix = $"Movie {movie.Title}:";
        _currentType = Type.Movie;
        _name = "";

        _show = new TvShowAppends();
        _season = new SeasonAppends();
        _episode = new EpisodeAppends();
    }

    private readonly List<PersonAppends?> _personAppends = [];

    private async Task FetchPeopleByCast(TMDBCast[] cast)
    {
        await Parallel.ForEachAsync(cast, async (person, _) =>
        {
            try
            {
                using PersonClient personClient = new PersonClient(person.Id);
                using var personTask = personClient.WithAllAppends();
                lock (_personAppends)
                {
                    _personAppends.Add(personTask.Result);
                }

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });
    }
    
    private async Task FetchPeopleByGuestStars(TMDBGuestStar[] cast)
    {
        await Parallel.ForEachAsync(cast, async (person, _) =>
        {
            try
            {
                using PersonClient personClient = new PersonClient(person.Id);
                using var personTask = personClient.WithAllAppends();
                lock (_personAppends)
                {
                    _personAppends.Add(personTask.Result);
                }

                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.MovieDb(e, LogLevel.Error);
            }
        });
    }

    private async Task FetchPeopleByCrew(TMDBCrew[] crew)
    {
        await Parallel.ForEachAsync(crew, async (person, _) =>
        {
            try
            {
                using PersonClient personClient = new(person.Id);
                using var personTask = personClient.WithAllAppends();
                lock (_personAppends)
                {
                    _personAppends.Add(personTask.Result);
                }

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
                await StoreCrew(_show.Credits.Crew, _show);

                //:TODO: Fix this
                // await StoreAggregateCredits(_show.AggregateCredits, _show);
                break;
            case Type.Season:
                if (_season is null) return;
                
                await FetchPeopleByCast(_season.Credits.Cast);
                await FetchPeopleByCrew(_season.Credits.Crew);

                await Store();

                await StoreCast(_season.Credits.Cast, _season);
                await StoreCrew(_season.Credits.Crew, _season);
                
                await StoreAggregateCredits(_season.AggregateCredits, _season);
                break;
            case Type.Episode:
                if (_episode is null) return;
                
                await FetchPeopleByCast(_episode.Cast);
                await FetchPeopleByCrew(_episode.Crew);
                await FetchPeopleByGuestStars(_episode.GuestStars);

                await Store();

                await StoreCast(_episode.Credits.Cast, _episode);
                await StoreCrew(_episode.Credits.Crew, _episode);
                
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

        await DispatchJobs();
    }

    private Task Store()
    {
        try
        {
            lock (_personAppends)
            {
                Person[] people = _personAppends.ConvertAll<Person>(x => new Person(x)).ToArray();
                
                _mediaContext.People.UpsertRange(people)
                    .On(p => new { p.Id })
                    .WhenMatched((ps, pi) => new Person
                    {
                        Id = pi.Id,
                        Adult = pi.Adult,
                        AlsoKnownAs = pi.AlsoKnownAs,
                        Biography = pi.Biography,
                        BirthDay = pi.BirthDay,
                        DeathDay = pi.DeathDay,
                        _gender = pi._gender,
                        Homepage = pi.Homepage,
                        ImdbId = pi.ImdbId,
                        KnownForDepartment = pi.KnownForDepartment,
                        Name = pi.Name,
                        PlaceOfBirth = pi.PlaceOfBirth,
                        Popularity = pi.Popularity,
                        Profile = pi.Profile,
                        TitleSort = pi.Name,
                        _colorPalette = pi._colorPalette,
                    })
                    .Run();

                StoreTranslations(_personAppends).Wait();

                Logger.MovieDb($@"{_logPrefix} {_name} stored {people.Length} people");
            }
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

        return Task.CompletedTask;
    }

    private Role[] GetRoles(Role[]? roles)
    {
        if (roles is null || roles.Length == 0) return Array.Empty<Role>();
        
        return _mediaContext.Roles
            .Where(role => roles.Select(r => r.CreditId).Contains(role.CreditId))
            .ToArray();
    }

    private async Task StoreCast(TMDBCast[] casts, dynamic? model)
    {
        try
        {
            Role[] roles = casts
                .ToList()
                .ConvertAll<Role>(x => new Role(x))
                .Where(cast => cast.CreditId is not null)
                .ToArray();
            
            await _mediaContext.Roles
                .UpsertRange(roles)
                .On(p => new { p.CreditId })
                .WhenMatched((rs, ri) => new Role
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    CreditId = ri.CreditId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"{_logPrefix} {_name} Roles stored");
        
            var crewArray = GetRoles(roles);
            
            var cast = casts
                .Where(cast => cast.CreditId is not "" && cast.CreditId is not null)
                .ToList()
                .ConvertAll<Cast>(x => new Cast(x, model, _movie, _show, _season, crewArray))
                .ToArray();

            UpsertCommandBuilder<Cast> query = _currentType switch
            {
                Type.Movie => _mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.MovieId, c.RoleId }),
                Type.TvShow => _mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.TvId, c.RoleId }),
                Type.Season => _mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.SeasonId, c.RoleId }),
                Type.Episode => _mediaContext.Casts.UpsertRange(cast).On(c => new { c.CreditId, c.EpisodeId, c.RoleId }),
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
                    RoleId = ci.RoleId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"{_logPrefix} {_name} Cast stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }

    }

    private Job[] GetJobs(Job[]? jobs)
    {
        if (jobs is null || jobs.Length == 0) return Array.Empty<Job>();
        
        return _mediaContext.Jobs
            .Where(job => jobs.Select(j => j.CreditId).Contains(job.CreditId))
            .ToArray();
    }
    
    private async Task StoreCrew(TMDBCrew[] crews, dynamic? model)
    {  
        try
        {
            Job[] jobs = crews
                .ToList()
                .ConvertAll<Job>(x => new Job(x))
                .Where(crew => crew.CreditId is not null)
                .ToArray();

            await _mediaContext.Jobs.UpsertRange(jobs)
                .On(p => new { p.CreditId })
                .WhenMatched((js, ji) => new Job
                {
                    Task = ji.Task,
                    CreditId = ji.CreditId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"{_logPrefix} {_name} Jobs stored");
              
                var jobArray = GetJobs(jobs);
                
                var crew = crews
                    .Where(crew => crew.CreditId is not "" && crew.CreditId is not null)
                    .ToList()
                    .ConvertAll<Crew>(x => new Crew(x, model, _movie, _show, _season, jobArray))
                    .ToArray();
            
            UpsertCommandBuilder<Crew> c1 = _currentType switch
            {
                Type.Movie => _mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.MovieId, c.JobId }),
                Type.TvShow => _mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.TvId, c.JobId }),
                Type.Season => _mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.SeasonId, c.JobId }),
                Type.Episode => _mediaContext.Crews.UpsertRange(crew).On(c => new { c.CreditId, c.EpisodeId, c.JobId }),
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
                    JobId = ci.JobId,
                })
                .RunAsync();

            Logger.MovieDb($@"{_logPrefix} {_name} Crew stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }
    
    private async Task StoreGuestStars(Providers.TMDB.Models.Shared.GuestStar[] guests, EpisodeAppends? episode)
    {
        try
        {
            var guestStars = guests
                .ToList()
                .ConvertAll<GuestStar>(x => new GuestStar(x, episode))
                .Where(crew => crew.CreditId is not "")
                .ToArray();
            
            await _mediaContext.GuestStars
                .UpsertRange(guestStars)
                .On(c => new { c.CreditId, c.EpisodeId })
                .WhenMatched((cs, ci) => new GuestStar
                {
                    Id = ci.Id,
                    CreditId = ci.CreditId,
                    PersonId = ci.PersonId,
                    EpisodeId = ci.EpisodeId,
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
            var roles = guests.ToList()
                .ConvertAll<Role>(x => new Role(x))
                .ToArray();
            
            await _mediaContext.Roles
                .UpsertRange(roles.Where(role => role.Character is not null && role.CreditId is not null))
                .On(p => new { p.GuestStarId })
                .WhenMatched((rs, ri) => new Role
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    CreditId = ri.CreditId,
                    GuestStarId = ri.GuestStarId,
                })
                .RunAsync();
            
            Logger.MovieDb($@"{_logPrefix} Roles stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreAggregateCredits(TvAggregatedCredits showAggregateCredits, TvShowAppends? show)
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
    
    private async Task StoreAggregateCredits(SeasonAggregatedCredits seasonAggregateCredits, SeasonAppends? season)
    {
        if (_show is null || season is null) return;
        try
        {
            await StoreAggregateCast(seasonAggregateCredits.Cast, season);
            await StoreAggregateCrew(seasonAggregateCredits.Crew, season);

            Logger.MovieDb($@"{_logPrefix} TvShow {_show.Name}, Season {season.SeasonNumber}: AggregateCredits stored");
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreAggregateCast(AggregatedCast[] aggregatedCasts, dynamic? model)
    {
        List<TMDBCast> cast = [];
        
        cast.AddRange(aggregatedCasts.Select(aggregatedCast => new TMDBCast
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

        foreach (var aggregatedCast in aggregatedCasts)
        {
            await StoreRoles(aggregatedCast.Roles);
        }
    }

    private async Task StoreAggregateCrew(AggregatedCrew[] aggregatedCrews, dynamic? model)
    {
        List<TMDBCrew> crew = [];
        
        crew.AddRange(aggregatedCrews.Select(aggregatedCrew => new TMDBCrew
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
            Order = aggregatedCrew.Order,
        }));

        await StoreCrew(crew.ToArray(), model);
        
        foreach (var aggregatedCrew in aggregatedCrews)
        {
            await StoreJobs(aggregatedCrew.Jobs);
        }
    }

    private async Task StoreRoles(AggregatedCreditRole[] role)
    {
        Role[] roles = role.ToList()
            .ConvertAll<Role>(x => new Role(x)).ToArray();

        try
        {
            await _mediaContext.Roles
                .UpsertRange(roles.Where(r => r.Character is not null && r.CreditId is not null))
                .On(p => new { p.CreditId })
                .WhenMatched((rs, ri) => new Role()
                {
                    EpisodeCount = ri.EpisodeCount,
                    Character = ri.Character,
                    CreditId = ri.CreditId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task StoreJobs(AggregatedCrewJob[] job)
    {
        Job[] jobs = job.ToList()
            .ConvertAll<Job>(x => new Job(x)).ToArray();

        try
        {
            await _mediaContext.Jobs.UpsertRange(jobs)
                .On(p => new { p.CreditId })
                .WhenMatched((js, ji) => new Job()
                {
                    Task = ji.Task,
                    EpisodeCount = ji.EpisodeCount,
                    CreditId = ji.CreditId,
                })
                .RunAsync();
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, LogLevel.Error);
        }
    }

    private async Task DispatchJobs()
    {
        lock (_personAppends)
        {
            foreach (var person in _personAppends)
            {
                if (person is null) continue;
                
                ColorPaletteJob colorPaletteJob = new ColorPaletteJob(id:person.Id, model:"person");
                JobDispatcher.Dispatch(colorPaletteJob, "data");

                foreach (var image in person.Images.Profiles)
                {
                    if (string.IsNullOrEmpty(image.FilePath)) continue;
                    
                    ColorPaletteJob colorPaletteJob2 = new ColorPaletteJob(filePath:image.FilePath, model:"image");
                    JobDispatcher.Dispatch(colorPaletteJob2, "data");
                }
            }
        }

        await Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        var person = await mediaContext.People
            .FirstOrDefaultAsync(e => e.Id == id);

        if(person is null) return;

        lock (person)
        {
            if (person is { _colorPalette: "", Profile: not null })
            {
                var palette = ImageLogic.GenerateColorPalette(profilePath: person.Profile).Result;
                person._colorPalette = palette;
                mediaContext.SaveChanges();
            }
            
        }
    }
    
    private async Task StoreTranslations(List<PersonAppends?> personAppends)
    {
        try
        {
            List<Translation> translations = [];

            Parallel.ForEach(personAppends, person =>
            {
                if (person is null) return;
                
                lock (translations)
                {
                    translations.AddRange(person.Translations.Translations.ToList()
                        .ConvertAll<Translation>(x => new Translation(x, person)));
                }
            });
        
            await _mediaContext.Translations
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
                    PersonId = ti.PersonId,
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
        lock (_personAppends)
        {
            _personAppends.Clear();
        }

        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}