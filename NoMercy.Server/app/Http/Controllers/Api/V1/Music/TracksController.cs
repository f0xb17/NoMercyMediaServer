using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[Tags("Music Albums")]
[Authorize, Route("api/v{Version:apiVersion}/music/tracks/{id:guid}")]
public class TracksController: Controller
{
    [HttpPost]
    [Route("like")]
    public IActionResult Like(Guid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
    
    [HttpPost]
    [Route("lyrics")]
    public IActionResult Lyrics(Guid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
    
    [HttpPost]
    [Route("playback")]
    public IActionResult Playback(Guid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
}