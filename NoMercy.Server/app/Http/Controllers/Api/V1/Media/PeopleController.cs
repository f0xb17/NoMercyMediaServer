using System.Net.Http.Headers;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media;

[ApiController]
[Tags("Media People")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/people")]
public class PeopleController : Controller
{
    [HttpGet]
    public Task<object> Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        using HttpClient client = new();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", (string)HttpContext.Request.Headers["Authorization"]!);
        
        return Task.FromResult<object>(client.GetAsync(
                "https://192-168-2-201.1968dcdc-bde6-4a0f-a7b8-5af17afd8fb6.nomercy.tv:7635/api/people")
            .Result.Content.ReadAsStringAsync().Result);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public Task<object> Show(int id)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        using HttpClient client = new();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", (string)HttpContext.Request.Headers["Authorization"]!);
        
        return Task.FromResult<object>(client.GetAsync(
                $"https://192-168-2-201.1968dcdc-bde6-4a0f-a7b8-5af17afd8fb6.nomercy.tv:7635/api/people/{id}")
            .Result.Content.ReadAsStringAsync().Result);
    }
    
}