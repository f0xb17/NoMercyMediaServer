using Microsoft.AspNetCore.Authorization;
using NoMercy.Providers.TMDB.Client;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Server.Controllers.Media;

[ApiController]
[Route("[controller]")]
[Authorize()]
public class HomeController: Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
    
    [HttpGet]
    [Route("/status")]
    public IActionResult Status()
    {
        return Ok();
    }
    
}