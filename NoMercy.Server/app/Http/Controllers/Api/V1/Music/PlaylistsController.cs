using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music Playlists")]
[Authorize, Route("api/v{Version:apiVersion}/music/playlists", Order = 3)]
public class PlaylistsController: Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult Show(Guid id)
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpPost]
    public IActionResult Create()
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        Guid userId = GetUserId();
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult Destroy(Guid id)
    {
        Guid userId = GetUserId();
        return Ok();
    }
}