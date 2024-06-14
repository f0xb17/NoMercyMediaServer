using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Music;

[ApiController]
[ApiVersion("1")]
[Tags("Music Genres")]
[Authorize]
[Route("api/v{Version:apiVersion}/music/genres", Order = 4)]
public class GenresController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult Show(Guid id)
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}