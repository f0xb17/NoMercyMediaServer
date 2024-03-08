using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media Specials")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/specials")]
public class SpecialController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        return Ok();
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Show(Ulid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        return Ok();
    }

    [HttpGet]
    [Route("{id}/available")]
    public IActionResult Available(Ulid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        return Ok();
    }

    [HttpGet]
    [Route("{id}/watch")]
    public IActionResult Watch(Ulid id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        return Ok();
    }
}