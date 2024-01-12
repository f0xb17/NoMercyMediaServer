using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Server.Controllers.Media;

[ApiController]
[Route("tv")]
[Authorize()]
public class TvShowController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        TvClient tvClient = new TvClient();
        
        var items = tvClient.Popular().Result;
        
        var tvs = new List<TvShowDetails>();
        Parallel.ForEach(items, (item,_) =>
        {
            TvClient tvClient2 = new TvClient(item.Id);
            var response = tvClient2.Details().Result;
            
            tvs.Add(response);
        });
        return Ok(tvs);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult Show(int id)
    {
        TvClient tvClient = new TvClient(id);
        TvShow tv = tvClient.Details().Result;
        
        return Ok(tv);
    }
    
    [HttpGet]
    [Route("{id}/details")]
    public IActionResult Details(int id)
    {
        TvClient tvClient = new TvClient(id);
        TvShowAppends tv = tvClient.WithAllAppends().Result;
        
        return Ok(tv);
    }
}