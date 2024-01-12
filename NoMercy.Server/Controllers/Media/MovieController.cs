using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.Movies;

namespace NoMercy.Server.Controllers.Media;

[ApiController]
[Route("movies")]
[Authorize()]
public class MovieController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        MovieClient movieClient = new MovieClient();
        
        var items = movieClient.Popular().Result;
        
        var movies = new List<MovieDetails>();
        Parallel.ForEach(items, (item,_) =>
        {
            MovieClient movieClient2 = new MovieClient(item.Id);
            var response = movieClient2.Details().Result;
            
            movies.Add(response);
        });
        
        return Ok(movies);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult Show(int id)
    {
        MovieClient movieClient = new MovieClient(id);
        Movie movie = movieClient.Details().Result;
        
        return Ok(movie);
    }
    
    [HttpGet]
    [Route("{id}/details")]
    public IActionResult Details(int id)
    {
        MovieClient movieClient = new MovieClient(id);
        MovieAppends movie = movieClient.WithAllAppends().Result;
        
        return Ok(movie);
    }
}