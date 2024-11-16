using System.ComponentModel.DataAnnotations.Schema;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Networking;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Media;

[ApiController]
[Tags("Media")]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}")]
public class HomeController(MediaContext mediaContext) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PageRequestDto request)
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view home");

        string language = Language();


        List<GenreRowDto<GenreRowItemDto>> genres = [];
        List<int> movieIds = [];
        List<int> tvIds = [];

        List<Genre> genreItems = await HomeResponseDto
            .GetHome(mediaContext, userId, language, request.Take, request.Page);

        foreach (Genre genre in genreItems)
        {
            string? name = genre.Translations.FirstOrDefault()?.Name ?? genre.Name;
            GenreRowDto<GenreRowItemDto> genreRowDto = new()
            {
                Title = name,
                MoreLink = $"/genres/{genre.Id}",
                Id = genre.Id.ToString(),

                Source = genre.GenreMovies.Select(movie => new HomeSourceDto(movie.MovieId, "movie"))
                    .Concat(genre.GenreTvShows.Select(tv => new HomeSourceDto(tv.TvId, "tv")))
                    .Randomize()
                    .Take(36)
            };

            tvIds.AddRange(genreRowDto.Source
                .Where(source => source.MediaType == "tv")
                .Select(source => source.Id));

            movieIds.AddRange(genreRowDto.Source
                .Where(source => source.MediaType == "movie")
                .Select(source => source.Id));

            genres.Add(genreRowDto);
        }

        List<Tv> tvData = [];
        await foreach (Tv tv in Queries.GetHomeTvs(mediaContext, tvIds, language))
        {
            tvData.Add(tv);
        }

        List<Movie> movieData = [];
        await foreach (Movie movie in Queries.GetHomeMovies(mediaContext, movieIds, language))
        {
            movieData.Add(movie);
        }

        foreach (GenreRowDto<GenreRowItemDto> genre in genres)
            genre.Items = genre.Source
                .Select(source =>
                {
                    switch (source.MediaType)
                    {
                        case "tv":
                        {
                            Tv? tv = tvData.FirstOrDefault(tv => tv.Id == source.Id);
                            return tv?.Id == null
                                ? null
                                : new GenreRowItemDto(tv,
                                    language);
                        }
                        case "movie":
                        {
                            Movie? movie = movieData.FirstOrDefault(movie => movie.Id == source.Id);
                            return movie?.Id == null
                                ? null
                                : new GenreRowItemDto(movie,
                                    language);
                        }
                        default:
                        {
                            return null;
                        }
                    }
                })
                .Where(genreRow => genreRow != null);

        return GetPaginatedResponse(genres, request);
    }

    [HttpGet]
    [Route("home")]
    public async Task<IActionResult> ContinueWatching2()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return Unauthorized(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "You do not have permission to view continue watching"
            });

        string language = Language();
        string country = Country();

        IEnumerable<UserData> continueWatching = Queries
            .GetContinueWatching(mediaContext, userId, language, country);

        List<GenreRowDto<GenreRowItemDto>> genres = [];

        List<int> movieIds = [];
        List<int> tvIds = [];

        HashSet<Genre> genreItems = Queries
            .GetHome(mediaContext, userId, language, 300);

        foreach (Genre genre in genreItems)
        {
            IEnumerable<HomeSourceDto> movies = genre
                .GenreMovies.Select(movie => new HomeSourceDto(movie.MovieId, "movie"));

            IEnumerable<HomeSourceDto> tvs = genre
                .GenreTvShows.Select(tv => new HomeSourceDto(tv.TvId, "tv"));

            string? name = genre.Translations.FirstOrDefault()?.Name ?? genre.Name;
            GenreRowDto<GenreRowItemDto> genreRowDto = new()
            {
                Title = name,
                MoreLink = $"/genres/{genre.Id}",
                Id = genre.Id.ToString(),
                Source = movies
                    .Concat(tvs)
                    .Randomize()
                    .Take(28)
            };

            tvIds.AddRange(genreRowDto.Source
                .Where(source => source.MediaType == "tv")
                .Select(source => source.Id));

            movieIds.AddRange(genreRowDto.Source
                .Where(source => source.MediaType == "movie")
                .Select(source => source.Id));

            genres.Add(genreRowDto);
        }

        List<Tv> tvData = [];
        await foreach (Tv tv in Queries.GetHomeTvs(mediaContext, tvIds, language))
        {
            tvData.Add(tv);
        }

        List<Movie> movieData = [];
        await foreach (Movie movie in Queries.GetHomeMovies(mediaContext, movieIds, language))
        {
            movieData.Add(movie);
        }

        foreach (GenreRowDto<GenreRowItemDto> genre in genres)
            genre.Items = genre.Source
                .Select(source =>
                {
                    switch (source.MediaType)
                    {
                        case "tv":
                        {
                            Tv? tv = tvData.FirstOrDefault(tv => tv.Id == source.Id);
                            return tv?.Id == null
                                ? null
                                : new GenreRowItemDto(tv, language);
                        }
                        case "movie":
                        {
                            Movie? movie = movieData.FirstOrDefault(movie => movie.Id == source.Id);
                            return movie?.Id == null
                                ? null
                                : new GenreRowItemDto(movie, language);
                        }
                        default:
                        {
                            return null;
                        }
                    }
                })
                .Where(genreRow => genreRow != null);

        return Ok(new  Render
        {
            Data = [

                new ComponentDto<GenreRowItemDto>
                {
                    Component = "NMHomeCard",
                    Update =
                    {
                        When = "pageLoad",
                        Link = new Uri("/home/card", UriKind.Relative),
                    },
                    Props =
                    {
                        Data = genres.Randomize().FirstOrDefault()
                            ?.Items.Randomize().FirstOrDefault() ?? new GenreRowItemDto(),
                    }
                },

                new ComponentDto<Dictionary<string, object>>
                {
                    Component = "NMServerComponent",
                    Props =
                    {
                        Url = new Uri("MyDynamicComponent.js", UriKind.Relative),
                    }
                },

                new ComponentDto<ContinueWatchingItemDto>
                {
                    Component = "NMCarousel",
                    Update =
                    {
                        When = "pageLoad",
                        Link = new Uri("/home/continue", UriKind.Relative),
                    },
                    Props =
                    {
                        Title = "Continue watching".Localize(),
                        MoreLink = null,
                        Items = continueWatching
                            .Select(item => new ComponentDto<ContinueWatchingItemDto>
                            {
                                Component = "NMCard",
                                Props =
                                {
                                    Data = new ContinueWatchingItemDto(item, country),
                                    Watch = true,
                                    ContextMenuItems =
                                    [
                                        new Dictionary<string, object>
                                        {
                                            {"label", "Remove from watchlist".Localize()},
                                            {"icon", "mooooom-trash"},
                                            {"method", "DELETE"},
                                            {"confirm", "Are you sure you want to remove this from continue watching?".Localize()},
                                            {"args", new Dictionary<string, object>
                                            {
                                                {"url", new Uri($"/userdata/continue", UriKind.Relative)},
                                            }}
                                        }
                                    ]
                                }
                            })
                    }
                },

                ..genres.Select(genre => new ComponentDto<GenreRowItemDto>
                {
                    Component = "NMCarousel",
                    Props =
                    {
                        Title = genre.Title,
                        MoreLink = genre.MoreLink,
                        Items = genre.Items.Select(item => new ComponentDto<GenreRowItemDto>
                        {
                            Component = "NMCard",
                            Props =
                            {
                                Data = item ?? new GenreRowItemDto(),
                                Watch = true,
                            }
                        })
                    }
                })
            ]
        });
    }

    [NotMapped]
    public record CardRequestDto
    {
        [JsonProperty("replace_id")] public Ulid ReplaceId { get; set; }
    }

    [HttpPost]
    [Route("home/card")]
    public async Task<IActionResult> HomeCard([FromBody] CardRequestDto request)
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view home card");

        string language = Language();

        Tv? tv = await Queries.GetRandomTvShow(mediaContext, userId, language);

        Movie? movie = await Queries.GetRandomMovie(mediaContext, userId, language);

        List<GenreRowItemDto> genres = [];
        if (tv != null)
            genres.Add(new GenreRowItemDto(tv, language));

        if (movie != null)
            genres.Add(new GenreRowItemDto(movie, language));

        return Ok(new  Render
        {
            Data = [

                new ComponentDto<GenreRowItemDto>
                {
                    Component = "NMHomeCard",
                    Replacing = request.ReplaceId,
                    Update =
                    {
                        When = "pageLoad",
                        Link = new Uri("/home/card", UriKind.Relative),
                    },
                    Props =
                    {
                        Data = genres.Randomize().First(),
                    }
                },
            ]
        });
    }

    [HttpPost]
    [Route("home/continue")]
    public async Task<IActionResult> HomeContinue([FromBody] CardRequestDto request)
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view home card");

        string country = Country();
        string language = Language();

        IEnumerable<UserData> continueWatching = Queries.GetContinueWatching(mediaContext, userId, language, country);

        return Ok(new  Render
        {
            Data = [

                new ComponentDto<ContinueWatchingItemDto>
                {
                    Component = "NMCarousel",
                    Update =
                    {
                        When = "pageLoad",
                        Link = new Uri("/home/continue", UriKind.Relative),
                    },
                    Props =
                    {
                        Title = "Continue watching".Localize(),
                        MoreLink = null,
                        Items = continueWatching
                            .Select(item => new ComponentDto<ContinueWatchingItemDto>
                            {
                                Component = "NMCard",
                                Props =
                                {
                                    Data = new ContinueWatchingItemDto(item, country),
                                    Watch = true,
                                    ContextMenuItems =
                                    [
                                        new Dictionary<string, object>
                                        {
                                            {"label", "Remove from watchlist".Localize()},
                                            {"icon", "mooooom-trash"},
                                            {"method", "DELETE"},
                                            {"confirm", "Are you sure you want to remove this from continue watching?".Localize()},
                                            {"args", new Dictionary<string, object>
                                            {
                                                {"url", new Uri($"/userdata/continue", UriKind.Relative)},
                                            }}
                                        }
                                    ]
                                }
                            })
                    }
                },
            ]
        });
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("/status")]
    public IActionResult Status()
    {
        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "NoMercy is running!",
            Data = "v1"
        });
    }

    [HttpGet]
    [Route("screensaver")]
    public async Task<IActionResult> Screensaver()
    {
        Guid userId = User.UserId();
        if (!User.IsAllowed())
            return UnauthorizedResponse("You do not have permission to view screensaver");

        HashSet<Image> data = Queries.GetScreensaverImages(mediaContext, userId);

        IEnumerable<Image> logos = data.Where(image => image.Type == "logo");

        IEnumerable<ScreensaverDataDto> tvCollection = data
            .Where(image => image.TvId != null && image.Type == "backdrop")
            .DistinctBy(image => image.TvId)
            .Select(image => new ScreensaverDataDto(image, logos, "tv"));

        IEnumerable<ScreensaverDataDto> movieCollection = data
            .Where(image => image.MovieId != null && image.Type == "backdrop")
            .DistinctBy(image => image.MovieId)
            .Select(image => new ScreensaverDataDto(image, logos, "movie"));

        return Ok(new ScreensaverDto
        {
            Data = tvCollection
                .Concat(movieCollection)
                .Where(image => image.Meta?.Logo != null)
                .Randomize()
        });
    }
}
