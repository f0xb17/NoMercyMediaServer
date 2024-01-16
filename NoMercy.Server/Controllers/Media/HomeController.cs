using Microsoft.AspNetCore.Authorization;
using NoMercy.Providers.TMDB.Client;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Models.TV;
using NoMercy.Server.Jobs;

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
    
    [HttpPatch]
    [Route("/workers/{worker}/{count:int:min(0)}")]
    public IActionResult UpdateWorkers(string worker, int count)
    {
        if (QueueRunner.SetWorkerCount(worker, count))
        {
            return Ok($"{worker} worker count set to {count}");
        }

        return BadRequest($"{worker} worker count could not be set to {count}");
        
    }
    
}