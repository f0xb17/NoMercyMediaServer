using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using Special = NoMercy.Database.Models.Special;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class SpecialResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public SpecialResponseItemDto? Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, Ulid, string, string, Task<Special?>> GetSpecial =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id, string language, string country) =>
            mediaContext.Specials
            .AsNoTracking()
            
            .Where(special => special.Id == id)
            
            .Include(special => special.Items
                .OrderBy(specialItem => specialItem.Order)
            )
            
            .Include(special => special.SpecialUser
                .Where(specialUser => specialUser.UserId == userId)
            )
            
            .FirstOrDefault());
    
    
    public static readonly Func<MediaContext, Guid, IEnumerable<int>, string, string, IAsyncEnumerable<Movie>> GetSpecialMovies =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, IEnumerable<int> ids, string language, string country) =>
            mediaContext.Movies.AsNoTracking()
                
                .Where(movie => ids.Contains(movie.Id))
                
                .Include(movie => movie.Translations
                    .Where(translation => translation.Iso6391 == language)
                )
                
                .Include(movie => movie.MovieUser
                    .Where(movieUser => movieUser.UserId == userId)
                )
                
                .Include(movie => movie.CertificationMovies
                    .Where(certification => certification.Certification.Iso31661 == country || certification.Certification.Iso31661 == "US")
                )
                    .ThenInclude(certificationMovie => certificationMovie.Certification)
                
                .Include(movie => movie.VideoFiles)
                    .ThenInclude(file => file.UserData
                        .Where(userData => userData.UserId == userId)
                    )
                
                .Include(movie => movie.GenreMovies)
                    .ThenInclude(genreMovie => genreMovie.Genre)
                
                .Include(movie => movie.Cast
                        .OrderBy(castTv => castTv.Role.Order)
                    )
                    .ThenInclude(castTv => castTv.Person)
                
                .Include(movie => movie.Cast
                    .OrderBy(castTv => castTv.Role.Order)
                )
                    .ThenInclude(castTv => castTv.Role)
                
                .Include(movie => movie.Crew)
                    .ThenInclude(crew => crew.Person)
                .Include(movie => movie.Crew)
                    .ThenInclude(crew => crew.Job)
                
                .Include(movie => movie.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                    .OrderByDescending(image => image.VoteAverage)
                )
            );
            
    
    public static readonly Func<MediaContext, Guid, IEnumerable<int>, string, string, IAsyncEnumerable<Tv>> GetSpecialTvs =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, IEnumerable<int> ids, string language, string country) =>
            mediaContext.Tvs.AsNoTracking()
            
                .Where(tv => tv.Episodes.Any(e => ids.Contains(e.Id)))
                
                .Include(tv => tv.Translations
                    .Where(translation => translation.Iso6391 == language)
                )

                .Include(tv => tv.TvUser
                    .Where(tvUser => tvUser.UserId == userId)
                )
                
                .Include(tv => tv.GenreTvs)
                    .ThenInclude(genreTv => genreTv.Genre)
                    
                .Include(tv => tv.Cast
                    .OrderBy(castTv => castTv.Role.Order)
                )
                    .ThenInclude(castTv => castTv.Person)
                
                .Include(tv => tv.Cast
                    .OrderBy(castTv => castTv.Role.Order)
                )
                    .ThenInclude(castTv => castTv.Role)
                
                .Include(tv => tv.Crew)
                    .ThenInclude(crew => crew.Person)
                .Include(tv => tv.Crew)
                    .ThenInclude(crew => crew.Job)
                
                .Include(tv => tv.CertificationTvs
                    .Where(certification => certification.Certification.Iso31661 == country || certification.Certification.Iso31661 == "US")
                )
                    .ThenInclude(certificationTv => certificationTv.Certification)
            
                .Include(tv => tv.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                    .OrderByDescending(image => image.VoteAverage)
                )
                
                .Include(tv => tv.Episodes)
                    .ThenInclude(episode => episode.VideoFiles
                        .Where(file => file.Folder != null)
                    )
                        .ThenInclude(file => file.UserData
                            .Where(userData => userData.UserId == userId))
            );
            
    
    public static readonly Func<MediaContext, Guid, Ulid, Task<Special?>> GetSpecialAvailable =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id) =>
            mediaContext.Specials.AsNoTracking()
            
                .Where(special => special.Id == id)
            
                .Include(special => special.Items)
                    .ThenInclude(specialItem => specialItem.Movie)
                        .ThenInclude(movie => movie.VideoFiles)
                            .ThenInclude(file => file.UserData)
            
                .Include(special => special.Items)
                    .ThenInclude(specialItem => specialItem.Episode)
                        .ThenInclude(movie => movie.VideoFiles)
                            .ThenInclude(file => file.UserData)
            
                .FirstOrDefault());
            
    
    public static readonly Func<MediaContext, Guid, Ulid, string, Task<Special?>> GetSpecialPlaylist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id, string language) =>
            mediaContext.Specials.AsNoTracking()
            
            .Where(special => special.Id == id)
            
            .Include(special => special.Items
                .OrderBy(specialItem => specialItem.Order)
            )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
                        .ThenInclude(file => file.UserData
                            .Where(userData => userData.UserId == userId)
                        )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                    .ThenInclude(movie => movie.CertificationMovies)
                        .ThenInclude(certificationMovie => certificationMovie.Certification)
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                    .ThenInclude(movie => movie.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                    .ThenInclude(movie => movie.MovieUser
                        .Where(movieUser => movieUser.UserId == userId)
                    )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                    .ThenInclude(movie => movie.Translations
                        .Where(translation => translation.Iso6391 == language)
                    )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.VideoFiles)
                        .ThenInclude(file => file.UserData
                            .Where(userData => userData.UserId == userId)
                        )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Season)
                        .ThenInclude(episode => episode.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Translations
                        .Where(translation => translation.Iso6391 == language)
                    )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Tv)
                        .ThenInclude(episode => episode.Translations
                            .Where(translation => translation.Iso6391 == language)
                        )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Tv)
                        .ThenInclude(episode => episode.CertificationTvs)
                            .ThenInclude(certificationTv => certificationTv.Certification)
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Tv)
                        .ThenInclude(episode => episode.Images
                        .Where(image =>
                            (image.Type == "logo" && image.Iso6391 == "en")
                            || ((image.Type == "backdrop" || image.Type == "poster") &&
                                (image.Iso6391 == "en" || image.Iso6391 == null))
                        )
                        .OrderByDescending(image => image.VoteAverage)
                    )
            
            .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                    .ThenInclude(episode => episode.Tv)
                        .ThenInclude(episode => episode.TvUser
                            .Where(tvUser => tvUser.UserId == userId)
                        )
            
            .FirstOrDefault());
}

public class SpecialResponseItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("collection")] public IEnumerable<SpecialItemDto> Special { get; set; }
    [JsonProperty("number_of_episodes")] public int? NumberOfEpisodes { get; set; }
    [JsonProperty("have_episodes")] public int? HaveEpisodes { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("genres")] public IEnumerable<GenreDto> Genres { get; set; }

    [JsonProperty("cast")] public IEnumerable<PeopleDto> Cast { get; set; }
    [JsonProperty("crew")] public IEnumerable<PeopleDto> Crew { get; set; }
    [JsonProperty("backdrops")] public IEnumerable<ImageDto> Backdrops { get; set; }
    [JsonProperty("posters")] public IEnumerable<ImageDto> Posters { get; set; }
    
    [JsonProperty("contentRatings")] public IEnumerable<Certification?> ContentRatings { get; set; }
    
    public SpecialResponseItemDto(Special special, List<SpecialItemsDto> items)
    {

        List<SpecialItemDto> specialItems = [];
        foreach (var specialItem in special.Items)
        {
            if (specialItem.MovieId is not null)
            {
                var newItem = items.Find(i => i.Id == specialItem.MovieId);
                if (newItem is null) continue;
                
                SpecialItemDto item = new(newItem);
                specialItems.Add(item);
            }
            else
            {
                
                var newItem = items.FirstOrDefault(i => i.EpisodeIds.Contains(specialItem.EpisodeId ?? 0));
                if (newItem is null) continue;
                
                SpecialItemDto item = new(newItem);
                specialItems.Add(item);
            }
        }
        
        IEnumerable<PeopleDto> cast = items
            .SelectMany(tv => tv.Cast)
            .ToArray();
        
        IEnumerable<PeopleDto> crew = items
            .SelectMany(item => item.Crew)
            .ToArray();

        IEnumerable<ImageDto> posters = items
            .SelectMany(item => item.Posters)
            .ToArray();
        
        IEnumerable<ImageDto> backdrops = items
            .SelectMany(item => item.Backdrops)
            .ToArray();
        
        IEnumerable<GenreDto> genres = items
            .SelectMany(item => item.Genres)
            .ToArray();
        
        foreach (var item in items)
        {
            item.Posters = [];
            item.Backdrops = [];
            item.Cast = [];
            item.Crew = [];
            item.Genres = [];
        }
        
        Id = special.Id;
        Title = special.Title;
        Overview = special.Description;
        Backdrop = special.Backdrop?.Replace("https://storage.nomercy.tv/laravel", "");
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();
        Type = "specials";
        MediaType = "specials";
        ColorPalette = special.ColorPalette;
        Backdrops = backdrops;
        Posters = posters;
        Cast = cast.DistinctBy(c => c.Id);
        Crew = crew.DistinctBy(c => c.Id);
        Genres = genres.DistinctBy(genre => genre.Id);
        
        Favorite = special.SpecialUser.Count != 0;

        NumberOfEpisodes = special.Items.Select(item => item.Episode?.Tv?.NumberOfEpisodes).Count() + special.Items.Count(item => item.MovieId != null);
        HaveEpisodes = special.Items.Count;

        ContentRatings = items
            .Select(specialItem => specialItem.Rating)
            .DistinctBy(rating => rating?.Iso31661);

        Special = specialItems.DistinctBy(si => si.Id);

    }

}

public class SpecialItemsDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("episode_ids")] public int[] EpisodeIds { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public long Year { get; set; }

    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }
    [JsonProperty("backdrops")] public IEnumerable<ImageDto> Backdrops { get; set; }
    [JsonProperty("posters")] public IEnumerable<ImageDto> Posters { get; set; }
    
    [JsonProperty("cast")] public IEnumerable<PeopleDto> Cast { get; set; }
    [JsonProperty("crew")] public IEnumerable<PeopleDto> Crew { get; set; }

    [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
    public Certification? Rating { get; set; }

    [JsonProperty("videoId", NullValueHandling = NullValueHandling.Ignore)]
    public string? VideoId { get; set; }
    
    [JsonProperty("number_of_episodes")] public int? NumberOfEpisodes { get; set; }
    [JsonProperty("have_episodes")] public int HaveEpisodes { get; set; }

    public SpecialItemsDto()
    {
        //
    }
    public SpecialItemsDto(Movie movie)
    {
        string? title = movie.Translations.FirstOrDefault()?.Title;
        string? overview = movie.Translations.FirstOrDefault()?.Overview;

        Id = movie.Id;
        EpisodeIds = [];
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : movie.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : movie.Overview;

        Backdrop = movie.Backdrop;
        Favorite = movie.MovieUser.Count != 0;
        // Watched = movie.Watched;
        Logo = movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        
        Backdrops = movie.Images
            .Where(media => media.Type == "backdrop")
            .Take(2)
            .Select(media => new ImageDto(media));

        Posters = movie.Images
            .Where(media => media.Type == "poster")
            .Take(2)
            .Select(media => new ImageDto(media));

        MediaType = "movie";
        ColorPalette = movie.ColorPalette;
        Poster = movie.Poster;
        Type = "movie";
        Year = movie.ReleaseDate.ParseYear();
        
        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie.Genre))
            .ToArray();

        Rating = movie.CertificationMovies
            .Select(certificationMovie => certificationMovie?.Certification)
            .FirstOrDefault();
        
        NumberOfEpisodes = 1;
        HaveEpisodes = movie.VideoFiles.Count > 0 ? 1 : 0;

        VideoId = movie.Video;
        
        Cast = movie.Cast
            .Take(15)
            .Select(cast => new PeopleDto(cast));
        
        Crew = movie.Crew
            .Take(15)
            .Select(crew => new PeopleDto(crew));
    }

    public SpecialItemsDto(Tv tv)
    {
        string? title = tv.Translations?.FirstOrDefault()?.Title;
        string? overview = tv.Translations?.FirstOrDefault()?.Overview;

        Id = tv.Id;
        EpisodeIds = tv.Episodes?
            .Select(episode => episode.Id)
            .ToArray() ?? [];
        
        Title = !string.IsNullOrEmpty(title) 
            ? title 
            : tv.Title;
        Overview = !string.IsNullOrEmpty(overview) 
            ? overview 
            : tv.Overview;

        Backdrop = tv.Backdrop;
        Favorite = tv.TvUser.Count != 0;
        // Watched = tv.Watched;
        Logo = tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        
        Backdrops = tv.Images
            .Where(media => media.Type == "backdrop")
            .Take(2)
            .Select(media => new ImageDto(media));

        Posters = tv.Images
            .Where(media => media.Type == "poster")
            .Take(2)
            .Select(media => new ImageDto(media));

        MediaType = "tv";
        ColorPalette = tv.ColorPalette;
        Poster = tv.Poster;
        Type = "tv";
        Year = tv.FirstAirDate.ParseYear();
        
        Genres = tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv.Genre))
            .ToArray();

        Rating = tv.CertificationTvs
            .Select(certificationTv => certificationTv?.Certification)
            .FirstOrDefault();
        
        NumberOfEpisodes = tv.NumberOfEpisodes;
        
        HaveEpisodes = tv.Episodes?
            .Count(episode => episode.VideoFiles
                .Any(videoFile => videoFile.Folder != null)) ?? 0;

        // Watched = tv.Episodes
        //     .SelectMany(episode => episode.VideoFiles
        //         .Where(videoFile => videoFile.UserData.Any(userData => userData.UserId == userId)))
        //     .Count();

        VideoId = tv.Trailer;
        
        Cast = tv.Cast
            .Take(15)
            .Select(cast => new PeopleDto(cast));
        
        Crew = tv.Crew
            .Take(15)
            .Select(crew => new PeopleDto(crew));
    }
}

public class SpecialItemDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public long Year { get; set; }
    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }
    
    [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
    public Certification? Rating { get; set; }

    [JsonProperty("videoId", NullValueHandling = NullValueHandling.Ignore)]
    public string? VideoId { get; set; }
    
    [JsonProperty("number_of_episodes")] public int? NumberOfEpisodes { get; set; }
    [JsonProperty("have_episodes")] public int HaveEpisodes { get; set; }

    public SpecialItemDto(SpecialItemsDto item)
    {
        Id = item.Id;
        Title = item.Title;
        Overview = item.Overview;
        Backdrop = item.Backdrop;
        Favorite = item.Favorite;
        Logo = item.Logo;
        Genres = item.Genres;
        MediaType = item.MediaType;
        ColorPalette = item.ColorPalette;
        Poster = item.Poster;
        Type = item.Type;
        Year = item.Year;
        Rating = item.Rating;
        NumberOfEpisodes = item.NumberOfEpisodes;
        HaveEpisodes = item.HaveEpisodes;
        VideoId = item.VideoId;
    }

}