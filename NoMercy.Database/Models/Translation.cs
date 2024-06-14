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

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(TvId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(SeasonId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(EpisodeId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(MovieId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(CollectionId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(PersonId), nameof(Iso6391), nameof(Iso31661), IsUnique = true)]
[Index(nameof(ReleaseGroupId), nameof(Iso31661), IsUnique = true)]
[Index(nameof(ArtistId), nameof(Iso31661), IsUnique = true)]
[Index(nameof(AlbumId), nameof(Iso31661), IsUnique = true)]
public class Translation : Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("iso_639_1")] public string? Iso6391 { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("english_name")] public string? EnglishName { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }

    [JsonProperty("tv_id")] public int? TvId { get; set; }
    public Tv Tv { get; set; }

    [JsonProperty("season_id")] public int? SeasonId { get; set; }
    public Season Season { get; set; }

    [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
    public Episode Episode { get; set; }

    [JsonProperty("movie_id")] public int? MovieId { get; set; }
    public Movie Movie { get; set; }

    [JsonProperty("collection_id")] public int? CollectionId { get; set; }
    public Collection Collection { get; set; }

    [JsonProperty("person_id")] public int? PersonId { get; set; }
    public Person People { get; set; }
    
    [JsonProperty("release_group_id")] public Guid? ReleaseGroupId { get; set; }
    public ReleaseGroup ReleaseGroup { get; set; }
    
    [JsonProperty("artist_id")] public Guid? ArtistId { get; set; }
    public Artist Artist { get; set; }
    
    [JsonProperty("release_id")] public Guid? AlbumId { get; set; }
    public Album Album { get; set; }

    public Translation()
    {
    }

    public Translation(TmdbCombinedTranslation translation, TmdbTvShow show)
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

    public Translation(TmdbCombinedTranslation translation, TmdbSeasonAppends tmdbSeason)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name == "" ? null : translation.Name;
        Title = translation.Data.Title == "" ? null : translation.Data.Title;
        Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
        EnglishName = translation.EnglishName;
        Homepage = translation.Data.Homepage?.ToString();
        SeasonId = tmdbSeason.Id;
    }

    public Translation(TmdbCombinedTranslation translation, TmdbEpisodeAppends tmdbEpisode)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name == "" ? null : translation.Name;
        Title = translation.Data.Title == "" ? null : translation.Data.Title;
        Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
        EnglishName = translation.EnglishName;
        Homepage = translation.Data.Homepage?.ToString();
        EpisodeId = tmdbEpisode.Id;
    }

    public Translation(TmdbCombinedTranslation translation, TmdbMovieAppends tmdbMovie)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name == "" ? null : translation.Name;
        Title = translation.Data.Title == "" ? null : translation.Data.Title;
        Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
        EnglishName = translation.EnglishName;
        Homepage = translation.Data.Homepage?.ToString();
        MovieId = tmdbMovie.Id;
    }

    public Translation(TmdbCombinedTranslation translation, Collection collection)
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

    public Translation(TmdbCombinedTranslation translation, Person person)
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

    public Translation(TmdbPersonTranslation translation, TmdbPersonAppends? person)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name;
        Overview = translation.TmdbPersonTranslationData.Overview == "" ? null : translation.TmdbPersonTranslationData.Overview;
        EnglishName = translation.EnglishName;
        PersonId = person?.Id;
    }

    public Translation(TmdbCombinedTranslation translation, TmdbCollectionAppends tmdbCollection)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        Name = translation.Name == "" ? null : translation.Name;
        Title = translation.Data.Title == "" ? null : translation.Data.Title;
        Overview = translation.Data.Overview == "" ? null : translation.Data.Overview;
        EnglishName = translation.EnglishName;
        Homepage = translation.Data.Homepage?.ToString();
        CollectionId = tmdbCollection.Id;
    }
}