using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoMercy.Providers.TMDB.Client;
using NoMercy.Providers.TMDB.Models.People;

namespace NoMercy.Server.Controllers.Media;

[ApiController]
[Route("people")]
[Authorize()]
public class PeopleController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        PersonClient personClient = new PersonClient();
        
        var items = personClient.Popular().Result;
        
        var people = new List<PersonDetails>();
        Parallel.ForEach(items, (item,_) =>
        {
            PersonClient personClient2 = new PersonClient(item.Id);
            var response = personClient2.Details().Result;
            
            people.Add(response);
        });
        return Ok(people);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult Show(int id)
    {
        PersonClient personClient = new PersonClient(id);
        Person person = personClient.Details().Result;
        
        return Ok(person);
    }
    
    [HttpGet]
    [Route("{id}/details")]
    public IActionResult Details(int id)
    {
        PersonClient personClient = new PersonClient(id);
        PersonAppends person = personClient.WithAllAppends().Result;
        
        return Ok(person);
    }
}