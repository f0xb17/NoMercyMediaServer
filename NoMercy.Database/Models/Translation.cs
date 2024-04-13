#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.People;
using NoMercy.Providers.TMDB.Models.Season;
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
    public class Translation : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
        [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("english_name")] public string EnglishName { get; set; }
        [JsonProperty("title")] public string? Title { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }
        [JsonProperty("homepage")] public string? Homepage { get; set; }
        [JsonProperty("biography")] public string? Biography { get; set; }

        [JsonProperty("tv_id")] public int? TvId { get; set; }
        public virtual Tv Tv { get; set; }

        [JsonProperty("season_id")] public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }

        [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }

        [JsonProperty("movie_id")] public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [JsonProperty("collection_id")] public int? CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [JsonProperty("person_id")] public int? PersonId { get; set; }
        public virtual Person People { get; set; }

        public Translation()
        {
        }

        public Translation(CombinedTranslation translation, TvShow show)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            Biography = translation.Data.Biography;
            TvId = show.Id;
        }

        public Translation(CombinedTranslation translation, SeasonAppends season)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            SeasonId = season.Id;
        }

        public Translation(CombinedTranslation translation, EpisodeAppends episode)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            EpisodeId = episode.Id;
        }

        public Translation(CombinedTranslation translation, MovieAppends movie)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            MovieId = movie.Id;
        }

        public Translation(CombinedTranslation translation, Collection collection)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            CollectionId = collection.Id;
        }

        public Translation(CombinedTranslation translation, Person person)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            Biography = translation.Data.Biography;
            PersonId = person.Id;
        }

        public Translation(PersonTranslation translation, PersonAppends? person)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            PersonId = person?.Id;
        }

        public Translation(CombinedTranslation translation, CollectionAppends collection)
        {
            Iso31661 = translation.Iso31661;
            Iso6391 = translation.Iso6391;
            Name = translation.Name == "" ? null : translation.Name;
            Title = translation.Data.Title == "" ? null : translation.Data.Title;
            Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
            EnglishName = translation.EnglishName;
            Homepage = translation.Data.Homepage?.ToString();
            CollectionId = collection.Id;
        }
    }
}