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
[Authorize, Route("api/v{Version:apiVersion}/tv/{id:int}")]
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
            Available = tv?.Episodes.Select(episode => episode.VideoFiles).Any() ?? false
        };
    }
    
    [HttpGet]
    [Route("watch")]
    public Task<object> Watch(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        using HttpClient client = new();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", (string)HttpContext.Request.Headers["Authorization"]!);
        
        return Task.FromResult<object>(client.GetAsync(
                $"https://192-168-2-201.1968dcdc-bde6-4a0f-a7b8-5af17afd8fb6.nomercy.tv:7635/api/tv/{id}/watch")
            .Result.Content.ReadAsStringAsync().Result);
    }
}