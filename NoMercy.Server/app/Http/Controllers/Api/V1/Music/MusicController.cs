using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music")]
[Authorize, Route("api/v{Version:apiVersion}/music")]
public class MusicController: Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Guid userId = GetUserId();

        List<GenreRowDto<CarouselResponseItemDto>> list = [];
        
        MediaContext mediaContext = new();

        var favoriteArtists = await mediaContext.ArtistUser
            .AsNoTracking()
            .Where(artistUser => artistUser.UserId == userId)
            .Include(artistUser => artistUser.Artist)
                .ThenInclude(artist => artist.ArtistTrack)
                    .ThenInclude(artistTrack => artistTrack.Track)
            .ToListAsync();
            
        var favoriteAlbums = await mediaContext.AlbumUser
            .AsNoTracking()
            .Where(albumUser => albumUser.UserId == userId)
            .Include(albumUser => albumUser.Album)
                .ThenInclude(album => album.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Track)
            .ToListAsync();
            
        var playlists = await mediaContext.Playlists
            .AsNoTracking()
            .Where(playlist => playlist.UserId == userId)
            .Include(playlist => playlist.PlaylistTrack)
                .ThenInclude(playlistTrack => playlistTrack.Track)
                    .ThenInclude(track => track.ArtistTrack)
                        .ThenInclude(artistTrack => artistTrack.Artist)
            
            .Include(playlist => playlist.PlaylistTrack)
                .ThenInclude(playlistTrack => playlistTrack.Track)
                    .ThenInclude(track => track.AlbumTrack)
                        .ThenInclude(albumTrack => albumTrack.Album)
            
            .ToListAsync();

        var latestAlbums = await mediaContext.Albums
            .AsNoTracking()
            .Where(artist => artist.AlbumTrack
                .Any(at => at.Track.Duration != null))
            
            .Include(album => album.AlbumTrack
                .Where(at => at.Track.Duration != null))
                .ThenInclude(albumTrack => albumTrack.Track)
            
            .OrderByDescending(album => album.CreatedAt)
            .Distinct()
            .Take(36)
            .ToListAsync();
        
        var latestArtists = await mediaContext.Artists
            .AsNoTracking()
            .Where(artist => artist.ArtistTrack
                .Any(at => at.Track.Duration != null))
            
            .Include(artist => artist.ArtistTrack
                .Where(at => at.Track.Duration != null))
                .ThenInclude(artistTrack => artistTrack.Track)
            
            .OrderByDescending(artist => artist.CreatedAt)
            .Distinct()
            .Take(36)
            .ToListAsync();

        list.Add(new GenreRowDto<CarouselResponseItemDto>
        {
            Title = "Favorite Artists",
            MoreLink = "",
            Items = favoriteArtists
                .Select(artist => new CarouselResponseItemDto(artist))
                .ToList()!
        });

        list.Add(new GenreRowDto<CarouselResponseItemDto>
        {
            Title = "Favorite Albums",
            MoreLink = "",
            Items = favoriteAlbums
                .Select(album => new CarouselResponseItemDto(album))
                .ToList()!
        });

        list.Add(new GenreRowDto<CarouselResponseItemDto>
        {
            Title = "Playlists",
            MoreLink = "/music/collection/playlists",
            Items = playlists
                .Select(playlist => new CarouselResponseItemDto(playlist))
                .ToList()!
        });
        
        list.Add(new GenreRowDto<CarouselResponseItemDto>
        {
            Title = "Artists",
            MoreLink = "/music/collection/artists",
            Items = latestArtists
                .Select(artist => new CarouselResponseItemDto(artist))
                .ToList()!
        });
        
        list.Add(new GenreRowDto<CarouselResponseItemDto>
        {
            Title = "Albums",
            MoreLink = "/music/collection/albums",
            Items = latestAlbums
                .Select(album => new CarouselResponseItemDto(album))
                .ToList()!
        });
        
        HomeResponseDto<CarouselResponseItemDto> result = new()
        {
            Data = list!
        };
        
        return Ok(result);
    }
    
    [HttpPost]
    [Route("search")]
    public IActionResult Search()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpPost]
    [Route("search/{query}/{Type}")]
    public IActionResult TypeSearch(string query, string type)
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpPost]
    [Route("coverimage")]
    public IActionResult CoverImage()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpPost]
    [Route("images")]
    public IActionResult Images()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
}