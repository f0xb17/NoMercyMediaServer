using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.MusixMatch.Client;
using NoMercy.Providers.MusixMatch.Models;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Jobs;
using Track = NoMercy.Database.Models.Track;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[Tags("Music Tracks")]
[Authorize, Route("api/v{Version:apiVersion}/music/tracks")]
public class TracksController: Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<ArtistTrackDto> tracks = [];
        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (var track in TracksResponseDto.GetTracks(mediaContext, userId))
        {
            tracks.Add(new ArtistTrackDto(track.Track));
        }

        return Ok(new TracksResponseDto
        {
            Data = new TracksResponseItemDto
            {
                ColorPalette = new IColorPalettes(),
                Tracks = tracks
            }
        });
    }

    [HttpPost]
    [Route("{id:guid}/like")]
    public async Task<IActionResult> Value(Guid id, [FromBody] LikeRequestDto request)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var track = await mediaContext.Tracks
            .AsNoTracking()
            .Include(track => track.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Artist)
            .Include(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)
            .Where(track => track.Id == id)
            .FirstOrDefaultAsync();

        if (track is null)
        {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Track not found"
            });
        }

        if (request.Value)
        {
            await mediaContext.TrackUser
                .Upsert(new TrackUser(track.Id, userId))
                .On(m => new { m.TrackId, m.UserId })
                .WhenMatched(m => new TrackUser
                {
                    TrackId = m.TrackId,
                    UserId = m.UserId
                })
                .RunAsync();
        }
        else
        {
            var tvUser = await mediaContext.TrackUser
                .Where(tvUser => tvUser.TrackId == track.Id && tvUser.UserId == userId)
                .FirstOrDefaultAsync();

            if (tvUser is not null)
            {
                mediaContext.TrackUser.Remove(tvUser);
            }

            await mediaContext.SaveChangesAsync();
        }

        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto()
        {
            QueryKey = ["music", "albums", track.AlbumTrack.First().Album.Id]
        });
        Networking.SendToAll("RefreshLibrary", new RefreshLibraryDto()
        {
            QueryKey = ["music", "artists", track.ArtistTrack.First().Artist.Id]
        });
        
        return Ok(new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "{0} {1}",
            Args = new object[]
            {
                track.Name,
                request.Value ? "liked" : "unliked"
            }
        });
    }
    
    [HttpGet]
    [Route("{id:guid}/lyrics")]
    [Obsolete("Obsolete")]
    public async Task<IActionResult> Lyrics(Guid id)
    {
        MediaContext mediaContext = new();
        Track? track = await mediaContext.Tracks
            .Where(track => track.Id == id)
            
            .Include(track => track.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Artist)
            .Include(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)
            
            .FirstOrDefaultAsync();

        if (track is null)
        {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Track not found"
            });
        }
        
        if(track.Lyrics is not null)
        {
            return Ok(new DataResponseDto<Lyric[]>
            {
                Data = track.Lyrics,
            });
        }
        
        MusixmatchClient client = new(id);
        var lyrics = await client.SongSearch(new TrackSearchParameters()
        {
            Album = track.AlbumTrack.First().Album.Name,
            Artist = track.ArtistTrack.First().Artist.Name,
            Artists = track.ArtistTrack.Select(artistTrack => artistTrack.Artist.Name).ToArray(),
            Title = track.Name,
            Duration = track.Duration?.ToSeconds().ToString(),
            Sort = TrackSearchParameters.SortStrategy.TrackRatingDesc
        });
        
        var subtitles = lyrics?.Message?.Body?.MacroCalls?
            .TrackSubtitlesGet?.Message?.Body?.SubtitleList?.FirstOrDefault()?.Subtitle?.SubtitleBody;
        
        if (subtitles is null) {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Subtitle not found"
            });
        }
        
        track._lyrics = JsonConvert.SerializeObject(subtitles);
        track.UpdatedAt = DateTime.UtcNow;
        await mediaContext.SaveChangesAsync();
        
        return Ok(new DataResponseDto<Lyric[]>
        {
            Data = track.Lyrics,
        });
    }
    
    [HttpPost]
    [Route("{id:guid}/playback")]
    public async Task<IActionResult> Playback(Guid id)
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        var track = await mediaContext.Tracks
            .AsNoTracking()
            .Where(track => track.Id == id)
            .FirstOrDefaultAsync();

        if (track is null)
        {
            return NotFound(new StatusResponseDto<string>
            {
                Status = "error",
                Message = "Track not found"
            });
        }
        
        await mediaContext.MusicPlays
            .AddAsync(new MusicPlay(userId, track.Id));
        
        await mediaContext.SaveChangesAsync();
        
        return Ok(new StatusResponseDto<string> {
            Status = "ok",
            Message = "Playback recorded"
        });
    }
}