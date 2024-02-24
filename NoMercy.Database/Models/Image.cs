using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Collections;
using NoMercy.Providers.TMDB.Models.Episode;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Season;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(FilePath), nameof(TvId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(SeasonId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(EpisodeId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(MovieId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(CollectionId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(PersonId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(CastCreditId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(CrewCreditId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(ArtistId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(AlbumId), IsUnique = true)]
    [Index(nameof(FilePath), nameof(TrackId), IsUnique = true)]
    public class Image : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("file_path")] public string? FilePath { get; set; }

        [JsonProperty("aspect_ratio")] public double AspectRatio { get; set; }

        [JsonProperty("height")] public int? Height { get; set; }

        [JsonProperty("iso_639_1")] public string? Iso6391 { get; set; }

        [JsonProperty("file_type")] public string? Name { get; set; }

        [JsonProperty("site")] public string? Site { get; set; }

        [JsonProperty("size")] public int? Size { get; set; }

        [JsonProperty("type")] public string? Type { get; set; }

        [JsonProperty("width")] public int? Width { get; set; }

        [JsonProperty("vote_average")] public double? VoteAverage { get; set; }

        [JsonProperty("vote_count")] public int? VoteCount { get; set; }

        [JsonProperty("cast_credit_id")] public string? CastCreditId { get; set; }
        public virtual Cast? Cast { get; set; }
        
        [JsonProperty("crew_credit_id")] public string? CrewCreditId { get; set; }
        public virtual Crew? Crew { get; set; }
        
        [JsonProperty("person_id")] public int? PersonId { get; set; }
        public virtual Person Person { get; set; }
        
        [JsonProperty("artist_id")] public Guid? ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        
        [JsonProperty("album_id")] public Guid? AlbumId { get; set; }
        public virtual Album Album { get; set; }
        
        [JsonProperty("track_id")] public Guid? TrackId { get; set; }
        public virtual Track Track { get; set; }
        
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

        public Image()
        {
        }

        public Image(NoMercy.Providers.TMDB.Models.Shared.Image image, TvShowAppends? show, string type)
        {
            AspectRatio = image.AspectRatio;
            Height = image.Height;
            Iso6391 = image.Iso6391;
            FilePath = image.FilePath;
            Width = image.Width;
            VoteAverage = image.VoteAverage;
            VoteCount = image.VoteCount;
            TvId = show.Id;
            Type = type;
        }

        public Image(NoMercy.Providers.TMDB.Models.Shared.Image image, SeasonAppends? season, string type)
        {
            AspectRatio = image.AspectRatio;
            Height = image.Height;
            Iso6391 = image.Iso6391;
            FilePath = image.FilePath;
            Width = image.Width;
            VoteAverage = image.VoteAverage;
            VoteCount = image.VoteCount;
            SeasonId = season.Id;
            Type = type;
        }

        public Image(NoMercy.Providers.TMDB.Models.Shared.Image image, EpisodeAppends? episode, string type)
        {
            AspectRatio = image.AspectRatio;
            Height = image.Height;
            Iso6391 = image.Iso6391;
            FilePath = image.FilePath;
            Width = image.Width;
            VoteAverage = image.VoteAverage;
            VoteCount = image.VoteCount;
            EpisodeId = episode.Id;
            Type = type;
        }

        public Image(NoMercy.Providers.TMDB.Models.Shared.Image image, MovieAppends? movie, string type)
        {
            AspectRatio = image.AspectRatio;
            Height = image.Height;
            Iso6391 = image.Iso6391;
            FilePath = image.FilePath;
            Width = image.Width;
            VoteAverage = image.VoteAverage;
            VoteCount = image.VoteCount;
            MovieId = movie.Id;
            Type = type;
        }

        public Image(Providers.TMDB.Models.Shared.Image image, CollectionAppends? collection, string type)
        {
            AspectRatio = image.AspectRatio;
            FilePath = image.FilePath;
            Height = image.Height;
            Iso6391 = image.Iso6391;
            VoteAverage = image.VoteAverage;
            VoteCount = image.VoteCount;
            Width = image.Width;
            CollectionId = collection.Id;
            Type = type;
        }
    }
}