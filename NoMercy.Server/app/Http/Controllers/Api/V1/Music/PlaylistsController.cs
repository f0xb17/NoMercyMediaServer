using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO;
using NoMercy.Server.app.Http.Middleware;
using PlaylistResponseItemDto = NoMercy.Server.app.Http.Controllers.Api.V1.Music.DTO.PlaylistResponseItemDto;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music Playlists")]
[Authorize]
[Route("api/v{Version:apiVersion}/music/playlists", Order = 3)]
public class PlaylistsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.User.UserId();
        
        List<PlaylistResponseItemDto> playlists = [];
        
        await using MediaContext mediaContext = new();
        await foreach (var playlist in PlaylistResponseDto.GetPlaylists(mediaContext, userId))
            playlists.Add(new PlaylistResponseItemDto(playlist));

        return Ok(new PlaylistResponseDto
        {
            Data = playlists
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Show(Ulid id)
    {
        var userId = HttpContext.User.UserId();

        List<ArtistTrackDto> tracks = [];

        await using MediaContext mediaContext = new();
        var playlist = await PlaylistResponseDto.GetPlaylist(mediaContext, userId, id);
        
        if (playlist == null)
            return NotFound();
        
        return Ok(new TracksResponseDto
        {
            Data = new TracksResponseItemDto
            {
                Name = playlist.Name,
                Cover = playlist.Cover,
                Description = playlist.Description,
                ColorPalette = playlist.ColorPalette,
                Tracks = playlist.Tracks.Select(t => new ArtistTrackDto(t.Track, HttpContext.Request.Headers.AcceptLanguage[1])).ToList() ?? [],
            }
        });
    }

    [HttpPost]
    public IActionResult Create()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPatch]
    [Route("{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult Destroy(Guid id)
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}

public class PlaceholderResponse
{
    [JsonProperty("data")] public string[] Data { get; set; }
}