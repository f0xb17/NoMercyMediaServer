using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.Controllers.Media;

[ApiController]
[Route("[controller]")]
[Authorize()]
public class SpecialController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}