using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(TvId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    [Index(nameof(SeasonId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    [Index(nameof(EpisodeId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    [Index(nameof(MovieId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    [Index(nameof(CollectionId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    [Index(nameof(PersonId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
    public class Translation: Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Iso31661 { get; set; }
        public string Iso6391 { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public string? Homepage { get; set; }
        public string? Biography { get; set; }
        
        public int? TvId { get; set; }
        public int? SeasonId { get; set; }
        public int? EpisodeId { get; set; }
        public int? MovieId { get; set; }
        public int? CollectionId { get; set; }
        public int? PersonId { get; set; }
        
        public Translation()
        {
        }
        
        public Translation(CombinedTranslation translation, TvShow show)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            Biography = translation.Data?.Biography;
            TvId = show.Id;
        }
        
        public Translation(CombinedTranslation translation, Season season)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            SeasonId = season.Id;
        }
        
        public Translation(CombinedTranslation translation, Episode episode)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            EpisodeId = episode.Id;
        }
        
        public Translation(CombinedTranslation translation, Movie movie)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            MovieId = movie.Id;
        }
        
        public Translation(CombinedTranslation translation, Collection collection)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            CollectionId = collection.Id;
        }
        
        public Translation(CombinedTranslation translation, Person person)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            EnglishName = translation.EnglishName;
            Title = translation.Data?.Title;
            Overview = translation.Data?.Overview;
            Homepage = translation.Data?.Homepage?.ToString();
            Biography = translation.Data?.Biography;
            PersonId = person.Id;
        }
        
    }
}