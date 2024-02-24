using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[Tags("Music Albums")]
[Authorize, Route("api/v{Version:apiVersion}/music/albums")]
public class AlbumsController: Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult Show(Guid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
    
    [HttpPost]
    [Route("{id:guid}/like")]
    public IActionResult Like(Guid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
}