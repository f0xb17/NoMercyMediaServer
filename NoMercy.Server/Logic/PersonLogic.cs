using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;
using Person = NoMercy.Database.Models.Person;

namespace NoMercy.Server.Logic;

public class PersonLogic
{
    public PersonLogic()
    {
    }

    private static List<PersonAppends> PersonAppends { get; } = [];

    private static async Task FetchPeopleByCast(IEnumerable<AggregatedCast> cast)
    {    
        await Parallel.ForEachAsync(cast, (person, token) =>
        {
            try
            {
                PersonClient personClient = new PersonClient(person.Id);
                PersonAppends.Add(personClient.WithAllAppends().Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return default;
        });
    }
    private static async Task FetchPeopleByCrew(IEnumerable<AggregatedCrew> crew)
    {    
        await Parallel.ForEachAsync(crew, (person, token) =>
        {
            try
            {
                PersonClient personClient = new PersonClient(person.Id);
                PersonAppends.Add(personClient.WithAllAppends().Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return default;
        });
    }

    public static async Task FetchPeople(TvShowAppends show)
    {        
        await FetchPeopleByCast(show.AggregateCredits!.Cast);
        await FetchPeopleByCrew(show.AggregateCredits!.Crew);
        
        await Store();
    }

    private static async Task Store()
    {
        Person[] people = PersonAppends.ConvertAll<Person>(x => new Person(x)).ToArray();
        
        await MediaContext.Db.UpsertRange(people)
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
            })
            .RunAsync();
        
    }
    
    
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