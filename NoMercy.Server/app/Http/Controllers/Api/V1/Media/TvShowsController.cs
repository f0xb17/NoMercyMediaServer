using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media TV Shows")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/tv/{id:int}")] // match themoviedb.org API
public class TvShowsController : Controller
{
    [HttpGet]
    public async Task<InfoResponseDto> Tv(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            .Include(tv => tv.TvUser)
            .Include(tv => tv.Library)
                .ThenInclude(library => library.LibraryUsers)
            .Include(tv => tv.Media)
            .Include(tv => tv.AlternativeTitles)
            
            .Include(tv => tv.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            
            .Include(tv => tv.Images
                .Where(image =>
                    (image.Type == "logo" && image.Iso6391 == "en")
                    || ((image.Type == "backdrop" || image.Type == "poster") &&
                        (image.Iso6391 == "en" || image.Iso6391 == null))
                )
            )
            
            .Include(tv => tv.CertificationTvs)
                .ThenInclude(certificationTv => certificationTv.Certification)
            
            .Include(tv => tv.GenreTvs)
                .ThenInclude(genreTv => genreTv.Genre)
            .Include(tv => tv.KeywordTvs)
                .ThenInclude(keywordTv => keywordTv.Keyword)
            .Include(tv => tv.Cast)
                .ThenInclude(castTv => castTv.Person)
            .Include(tv => tv.Cast)
                .ThenInclude(castTv => castTv.Role)
            .Include(tv => tv.Crew)
                .ThenInclude(crewTv => crewTv.Person)
            .Include(tv => tv.Crew)
                .ThenInclude(crewTv => crewTv.Job)
            
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Translations
                    .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0])
                )
            
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Episodes)
                    .ThenInclude(episode => episode.Translations
                        .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0])
                    )
            
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Episodes)
                    .ThenInclude(episode => episode.VideoFiles)
                        .ThenInclude(file => file.UserData)
            
            .Include(tv => tv.Episodes)
                .ThenInclude(episode => episode.VideoFiles)
                    .ThenInclude(file => file.UserData)
            
            .Include(tv => tv.RecommendationFrom)
            .Include(tv => tv.SimilarFrom)
            .FirstOrDefaultAsync();

        return new InfoResponseDto
        {
            Data = tv is not null
                ? new InfoResponseItemDto(tv, HttpContext.Request.Headers.AcceptLanguage[1] ?? "US")
                : null
        };
    }

    [HttpGet]
    [Route("available")]
    public async Task<AvailableResponseDto> Available(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            .Where(tv => tv.Id == id)
            .Include(tv => tv.Episodes)
                .ThenInclude(tv => tv.VideoFiles)
            .FirstOrDefaultAsync();

        return new AvailableResponseDto
        {
            Available = tv?.Episodes.Any(episode => episode.VideoFiles.Count > 0) ?? false
        };
    }

    [HttpGet]
    [Route("watch")]
    public async Task<PlaylistResponseDto[]> Watch(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .Where(tv => tv.Library.LibraryUsers
                .FirstOrDefault(u => u.UserId == userId) != null)
            .Include(tv => tv.Media
                .Where(media => media.Type == "video"))
            .Include(tv => tv.Images
                .Where(image => image.Type == "logo"))
            .Include(tv => tv.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0]))
            .Include(tv => tv.Episodes)
                .ThenInclude(episode => episode.Season)
            .Include(tv => tv.Episodes)
                .ThenInclude(episode => episode.Translations
                .Where(translation => translation.Iso6391 == HttpContext.Request.Headers.AcceptLanguage[0])
            )
            .Include(tv => tv.Episodes)
                .ThenInclude(tv => tv.VideoFiles)
                .ThenInclude(file => file.UserData)
            .FirstOrDefaultAsync();

        return tv?.Episodes
            .Select(episode => new PlaylistResponseDto(episode))
            .ToArray() ?? [];
    }

    [HttpPost]
    [Route("like")]
    public async Task<StatusResponseDto<string>> Like(int id, [FromBody] LikeRequestDto request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        await using MediaContext mediaContext = new();
        var tv = await mediaContext.Tvs
            .AsNoTracking()
            .Where(tv => tv.Id == id)
            .FirstOrDefaultAsync();

        if (tv is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };
        }

        if (request.Like)
        {
            await mediaContext.TvUser.Upsert(new TvUser(tv.Id, userId))
                .On(m => new { m.TvId, m.UserId })
                .WhenMatched(m => new TvUser
                {
                    TvId = m.TvId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var tvUser = await mediaContext.TvUser
                .Where(tvUser => tvUser.TvId == tv.Id && tvUser.UserId == userId)
                .FirstOrDefaultAsync();

            if (tvUser is not null)
            {
                mediaContext.TvUser.Remove(tvUser);
            }

            await mediaContext.SaveChangesAsync();
        }

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                tv.Title ?? "unknown",
                request.Like ? "liked" : "unliked"
            }
        };
    }
}