using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
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
            
            .Include(tv => tv.Library)
                .ThenInclude(library => library.LibraryUsers)
            
            .Include(tv => tv.Media)
            .Include(tv => tv.AlternativeTitles)
            
            .Include(tv => tv.Translations
                .Where(translation => translation.Iso6391 == "en" || translation.Iso6391 == "nl"))
            
            .Include(tv => tv.Images
                .Where(image => 
                    (image.Type == "logo" && (image.Iso6391 == "en" || image.Iso6391 == null))
                    || ((image.Type == "backdrop" || image.Type == "poster") && (image.Iso6391 == "en" || image.Iso6391 == null))
                ))

            .Include(tv => tv.CertificationTvs)!
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
                .ThenInclude(season => season.Translations)
            
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Episodes)
                    .ThenInclude(episode => episode.Translations)
            
            .Include(tv => tv.Episodes)
                .ThenInclude(episode => episode.VideoFiles)
                    .ThenInclude(file => file.UserData)
            
            .Include(tv => tv.RecommendationFrom)
            
            .Include(tv => tv.SimilarFrom)
            
            .FirstOrDefaultAsync();
        
        return new InfoResponseDto
        {
            Data = tv is not null
                ? new InfoResponseItemDto(tv) 
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
            Available = tv?.Episodes
                .Select(episode => episode.VideoFiles)
                .Any() ?? false
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
                .Where(translation => translation.Iso6391 == "en" || translation.Iso6391 == "nl"))
            
            .Include(tv => tv.Seasons)
                .ThenInclude(season => season.Translations)
            
            .Include(tv => tv.Episodes)
                .ThenInclude(episode => episode.Season) 
                
            .Include(tv => tv.Episodes)
                .ThenInclude(tv => tv.VideoFiles)
                    .ThenInclude(file => file.UserData)
            
            .FirstOrDefaultAsync();

        return tv?.Episodes
            .Select(episode => new PlaylistResponseDto(episode))
            .ToArray() ?? [];
    }
}