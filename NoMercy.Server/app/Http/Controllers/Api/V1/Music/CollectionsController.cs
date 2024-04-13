using System.Security.Claims;
using System.Text.RegularExpressions;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music Collections")]
[Authorize, Route("api/v{Version:apiVersion}/music/collection", Order = 2)]
public class CollectionsController: Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    [Route("tracks")]
    public async Task<IActionResult> Tracks()
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

    [HttpGet]
    [Route("artists")]
    public async Task<IActionResult> Artists([FromQuery] FilterRequest request)
    {
        List<ArtistsResponseItemDto> artists = [];
        
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (var artist in ArtistsResponseDto.GetArtists(mediaContext, userId, request.Letter))
        {
            artists.Add(new ArtistsResponseItemDto(artist));
        }
        
        var tracks = mediaContext.ArtistTrack
            .Where(artistTrack => artists.Select(a => a.Id)
                .Contains(artistTrack.ArtistId))
            .Where(artistTrack => artistTrack.Track.Duration != null)
            .Include(artistTrack => artistTrack.Track)
            .ToList();
        
        foreach (var artist in artists)
        {
            artist.Tracks = tracks
                .DistinctBy(track => Regex.Replace(track.Track.Filename ?? "", @"[\d+-]\s", "").ToLower())
                .Count(track => track.ArtistId == artist.Id);
        }

        return Ok(new ArtistsResponseDto
        {
            Data = artists
                .Where(response => response.Tracks > 0)
        });
    }

    [HttpGet]
    [Route("albums")]
    public async Task<IActionResult> Albums([FromQuery] FilterRequest request)
    {
        List<AlbumsResponseItemDto> albums = [];
        Guid userId = GetUserId();

        await using MediaContext mediaContext = new();
        await foreach (var album in AlbumsResponseDto.GetAlbums(mediaContext, userId, request.Letter))
        {
            albums.Add(new AlbumsResponseItemDto(album));
        }
        
        var tracks = mediaContext.AlbumTrack
            .Where(albumTrack => albums.Select(a => a.Id)
                .Contains(albumTrack.AlbumId))
            .Where(albumTrack => albumTrack.Track.Duration != null)
            .Include(albumTrack => albumTrack.Track)
            .ToList();
        
        foreach (var album in albums)
        {
            album.Tracks = tracks
                .DistinctBy(track => Regex.Replace(track.Track.Filename ?? "", @"[\d+-]\s", "").ToLower())
                .Count(track => track.AlbumId == album.Id);
        }
        
        return Ok(new AlbumsResponseDto
        {
            Data = albums
                .Where(response => response.Tracks > 0)
        });
    }
    
    [HttpGet]
    [Route("playlists")]
    public IActionResult Playlists()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
}