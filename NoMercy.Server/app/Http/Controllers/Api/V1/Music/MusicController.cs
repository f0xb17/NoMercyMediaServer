using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music")]
[Authorize]
[Route("api/v{Version:apiVersion}/music")]
public class MusicController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.User.UserId();

        List<GenreRowDto<dynamic>> list = [];
        List<GenreRowDto<TopMusicDto<string>>> list2 = [];

        MediaContext mediaContext = new();

        var playlists = await mediaContext.Playlists
            .AsNoTracking()
            .Where(playlist => playlist.UserId == userId)
            .Where(artist => artist.Tracks
                .Any(artistTrack => artistTrack.Track.Duration != null))
            
            .Include(playlist => playlist.Tracks)
                .ThenInclude(playlistTrack => playlistTrack.Track)
                    .ThenInclude(track => track.ArtistTrack)
                        .ThenInclude(artistTrack => artistTrack.Artist)
                            .ThenInclude(artist => artist.Translations)
            
            .Include(playlist => playlist.Tracks)
                .ThenInclude(playlistTrack => playlistTrack.Track)
                    .ThenInclude(track => track.AlbumTrack)
                        .ThenInclude(albumTrack => albumTrack.Album)
                            .ThenInclude(artist => artist.Translations)
            
            .ToListAsync();

        var latestAlbums = await mediaContext.Albums
            .AsNoTracking()
            .Where(album => album.Cover != null)
            .Where(album => album.AlbumTrack.Count > 0)
            .Where(artist => artist.AlbumTrack
                .Any(artistTrack => artistTrack.Track.Duration != null))
            
            .Include(album => album.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Artist)
                    .ThenInclude(artist => artist.Translations)
            
            .Include(album => album.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Track)
            
            .OrderByDescending(album => album.CreatedAt)
            .Take(36)
            .ToListAsync();
        

        var latestArtists = await mediaContext.Artists
            .AsNoTracking()
            .Where(artist => artist.Cover != null)
            .Where(artist => artist.ArtistTrack.Count > 0)
            .Where(artist => artist.ArtistTrack
                .Any(artistTrack => artistTrack.Track.Duration != null))
            
            .Include(artist => artist.Translations)
            
            .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
            
            .OrderByDescending(artist => artist.CreatedAt)
            .Take(36)
            .ToListAsync();
        
        var favoriteArtists = await mediaContext.ArtistUser
            .AsNoTracking()
            .Where(artistUser => artistUser.UserId == userId)
            
            .Include(artistUser => artistUser.Artist)
                .ThenInclude(artist => artist.Translations)
            
            .Include(artistUser => artistUser.Artist)
                .ThenInclude(artist => artist.ArtistTrack)
                    .ThenInclude(artistTrack => artistTrack.Track)
            .ToListAsync();
        
        var favoriteAlbums = await mediaContext.AlbumUser
            .AsNoTracking()
            .Where(albumUser => albumUser.UserId == userId)
            
            .Include(albumUser => albumUser.Album)
                .ThenInclude(artist => artist.Translations)
            
            .Include(albumUser => albumUser.Album)
                .ThenInclude(album => album.AlbumTrack)
                    .ThenInclude(albumTrack => albumTrack.Track)
            
            .Include(albumUser => albumUser.Album)
                .ThenInclude(album => album.AlbumArtist)
                    .ThenInclude(albumArtist => albumArtist.Artist)
                        .ThenInclude(artist => artist.Translations)
            
            .ToListAsync();
        
        var plays = await mediaContext.MusicPlays
            .Where(musicPlay => musicPlay.UserId == userId)
            
            .Include(musicPlay => musicPlay.Track)
                .ThenInclude(track => track.ArtistTrack)
                    .ThenInclude(artistTrack => artistTrack.Artist)
                        .ThenInclude(artist => artist.Translations)
            
            .Include(musicPlay => musicPlay.Track)
                .ThenInclude(track => track.ArtistTrack)
                    .ThenInclude(artistTrack => artistTrack.Artist)
                        .ThenInclude(artist => artist.ArtistTrack)
                            .ThenInclude(artistTrack => artistTrack.Track)
            
            .Include(musicPlay => musicPlay.Track)
                .ThenInclude(track => track.AlbumTrack)
                    .ThenInclude(artistTrack => artistTrack.Album)
                        .ThenInclude(artist => artist.Translations)
            
            .Include(musicPlay => musicPlay.Track)
                .ThenInclude(track => track.AlbumTrack)
                    .ThenInclude(artistTrack => artistTrack.Album)
                        .ThenInclude(album => album.AlbumArtist)
                            .ThenInclude(albumArtist => albumArtist.Artist)
            .ThenInclude(artist => artist.Translations)
            
            .Include(musicPlay => musicPlay.Track)
                .ThenInclude(track => track.PlaylistTrack)
                    .ThenInclude(artistTrack => artistTrack.Playlist)
                        .ThenInclude(playlist => playlist.Tracks)
                            .ThenInclude(playlistTrack => playlistTrack.Track)
            
            .ToListAsync();

        var favoriteArtist = plays
            .SelectMany(p => p.Track.ArtistTrack)
            .Select(a => new TopMusicDto<string>
            {
                Id = a.Artist.Id.ToString(),
                Name = a.Artist.Name,
                ColorPalette = a.Artist.ColorPalette,
                Type = "artists",
                Cover = a.Artist.Cover,
                Tracks = a.Artist.ArtistTrack.Count,
                Items = a.Artist.ArtistTrack.Select(at => at.Track.Name)
            })
            .GroupBy(a => a.Name)
            .MaxBy(g => g.Count())?
            .FirstOrDefault();
        
        var favoriteAlbum = plays
            .SelectMany(p => p.Track.AlbumTrack)
            .Select(a => new TopMusicDto<string>
            {
                Id = a.Album.Id.ToString(),
                Name = a.Album.Name,
                ColorPalette = a.Album.ColorPalette,
                Type = "albums",
                Cover = a.Album.Cover,
                Tracks = a.Album.Tracks,
                Items = a.Album.AlbumArtist.Select(at => at.Artist.Name)
            })
            .GroupBy(a => a.Name)
            .MaxBy(g => g.Count())?
            .FirstOrDefault();
        
        var favoritePlaylist = plays
            .SelectMany(p => p.Track.PlaylistTrack)
            .Select(p => new TopMusicDto<string>
            {
                Id = p.Playlist.Id.ToString(),
                Name = p.Playlist.Name,
                ColorPalette = p.Playlist.ColorPalette,
                Type = "playlists",
                Cover = p.Playlist.Cover,
                Tracks = p.Playlist.Tracks.Count,
                Items = p.Playlist.Tracks.Select(pt => pt.Track.Name)
            })
            .GroupBy(a => a.Name)
            .MaxBy(g => g.Count())?
            .FirstOrDefault();

        list.Add(new GenreRowDto<dynamic>
        {
            Title = null,
            MoreLink = "",
            Items =
            [
                favoriteArtist,
                favoriteAlbum,
                favoritePlaylist,
            ]
        });
        
        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Favorite Artists",
            MoreLink = "",
            Items = favoriteArtists
                .Select(artist => new CarouselResponseItemDto(artist))!
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Favorite Albums",
            MoreLink = "",
            Items = favoriteAlbums
                .Select(album => new CarouselResponseItemDto(album))!
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Playlists",
            MoreLink = "app.music.playlists",
            Items = playlists
                .Select(playlist => new CarouselResponseItemDto(playlist))
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Artists",
            MoreLink = "app.music.artists",
            Items = latestArtists
                .Select(artist => new CarouselResponseItemDto(artist))
        });

        list.Add(new GenreRowDto<dynamic>
        {
            Title = "Albums",
            MoreLink = "app.music.albums",
            Items = latestAlbums
                .Select(album => new CarouselResponseItemDto(album))!
        });

        return Ok(new HomeResponseDto<dynamic>
        {
            Data = list
        });
    }

    [HttpPost]
    [Route("search")]
    public IActionResult Search()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("search/{query}/{Type}")]
    public IActionResult TypeSearch(string query, string type)
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("coverimage")]
    public IActionResult CoverImage()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("images")]
    public IActionResult Images()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}