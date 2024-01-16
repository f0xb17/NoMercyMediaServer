using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Jobs;
using Exception = System.Exception;
using Person = NoMercy.Database.Models.Person;

namespace NoMercy.Server.Logic;

public class PersonLogic(TvShowAppends show)
{
    private readonly List<PersonAppends> _personAppends = [];

    private async Task FetchPeopleByCast()
    {
        await Parallel.ForEachAsync(show.AggregateCredits.Cast, async (person, _) =>
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
                Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
            }
        });
    }

    private async Task FetchPeopleByCrew()
    {
        await Parallel.ForEachAsync(show.AggregateCredits.Crew, async (person, _) =>
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
                Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
            }
        });
    }

    public async Task FetchPeople()
    {
        await FetchPeopleByCast();
        await FetchPeopleByCrew();

        await Store();
        
        await DispatchJobs();
    }

    private Task Store()
    {
        try
        {
            lock (_personAppends)
            {
                Person[] people = _personAppends.ConvertAll<Person>(x => new Person(x)).ToArray();

                using MediaContext mediaContext = new MediaContext();
                mediaContext.People.UpsertRange(people)
                    .On(p => new { p.Id })
                    .WhenMatched((ps, pi) => new Person()
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

                Logger.MovieDb($@"TvShow {show.Name} stored {people.Length} people");
                
            }
        }
        catch (Exception e)
        {
            Logger.MovieDb(e, NoMercy.Helpers.LogLevel.Error);
            throw;
        }
        
        return Task.CompletedTask;
    }
    
    private async Task DispatchJobs()
    {
        lock (_personAppends)
        {
            foreach (var person in _personAppends)
            {
                ColorPaletteJob colorPaletteJob = new(person.Id, "person");
                JobDispatcher.Dispatch(colorPaletteJob, "data");
            }
        }

        await Task.CompletedTask;
    }

    public static async Task GetPalette(int id)
    {
        await using MediaContext mediaContext = new MediaContext();

        var person = await mediaContext.People
            .FirstOrDefaultAsync(e => e.Id == id);

        if (person is { _colorPalette: "", Profile: not null })
        {
            var palette = await ImageLogic.GenerateColorPalette(profilePath: person.Profile);
            person._colorPalette = palette;
            await mediaContext.SaveChangesAsync();
        }
    }


    // private async Task FetchTranslations()
    // {
    //     try
    //     {
    //        await using MediaContext mediaContext = new MediaContext();

    //         var translations = Show!.Translations!.Translations.ToList()
    //             .ConvertAll<Translation>(x => new Translation(x, Show!)).ToArray();
    //     
    //         await mediaContext.Translations
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
        lock (_personAppends)
        {
            _personAppends.Clear();
        }

        GC.Collect();
        GC.WaitForFullGCComplete();
    }
}