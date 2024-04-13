using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Jobs;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music Artists")]
[Authorize, Route("api/v{Version:apiVersion}/music/artists")]
public class ArtistsController: Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }
    
    [HttpGet]
    public async Task<ArtistsResponseDto> Index([FromQuery] FilterRequest request)
    {
        List<ArtistsResponseItemDto> artists = [];
        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (var artist in ArtistsResponseDto.GetArtists(mediaContext, userId, request.Letter))
        {
            artists.Add(new ArtistsResponseItemDto(artist));
        }

        var tracks = mediaContext.ArtistTrack
            .Where(artistTrack => artists.Select(a => a.Id).Contains(artistTrack.ArtistId))
            .Where(artistTrack => artistTrack.Track.Duration != null)
            .ToList();
        
        foreach (var artist in artists)
        {
            artist.Tracks = tracks.Count(track => track.ArtistId == artist.Id);
        }

        return new ArtistsResponseDto
        {
            Data = artists
                .Where(response => response.Tracks > 0)
        };
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ArtistResponseDto> Show(Guid id)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var artist = await ArtistResponseDto.GetArtist(mediaContext, userId, id);
        
        if (artist is null)
        {
            return new ArtistResponseDto();
        }
        
        return new ArtistResponseDto
        {
            Data = new ArtistResponseItemDto(artist)
        };
    }
    
    [HttpPost]
    [Route("{id:guid}/like")]
    public async Task<StatusResponseDto<string>> Like(Guid id, [FromBody] LikeRequestDto request)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var artist = await mediaContext.Artists
            .AsNoTracking()
            .Where(artistUser => artistUser.Id == id)
            .FirstOrDefaultAsync();

        if (artist is null)
        {
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Tv not found"
            };
        }

        if (request.Value)
        {
            await mediaContext.ArtistUser
                .Upsert(new ArtistUser(artist.Id, userId))
                .On(m => new { m.ArtistId, m.UserId })
                .WhenMatched(m => new ArtistUser
                {
                    ArtistId = m.ArtistId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var tvUser = await mediaContext.ArtistUser
                .Where(tvUser => tvUser.ArtistId == artist.Id && tvUser.UserId == userId)
                .FirstOrDefaultAsync();

            if (tvUser is not null)
            {
                mediaContext.ArtistUser.Remove(tvUser);
            }

            await mediaContext.SaveChangesAsync();
        }
        
        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto()
        {
            QueryKey = ["music", "artists", artist.Id]
        });

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                artist.Name,
                request.Value ? "liked" : "unliked"
            }
        };
    }

    [HttpPost]
    [Route("{id:guid}/rescan")]
    public async Task<StatusResponseDto<string>> Like(Guid id)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Rescan started",
            Args = []
        };
    }
}