using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Configuration")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/configuration", Order = 10)]
public class ConfigurationController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpPost]
    public IActionResult Store()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpPatch]
    public IActionResult Update()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpGet]
    [Route("languages")]
    public async Task<List<LanguageDto>> Languages()
    {
        await using MediaContext context = new();
        List<Language> languages = await context.Languages
            .ToListAsync();

        return languages.Select(language => new LanguageDto
        {
            Id = language.Id,
            Iso6391 = language.Iso6391,
            EnglishName = language.EnglishName,
            Name = language.Name
        }).ToList();
    }

    [HttpGet]
    [Route("countries")]
    public async Task<List<CountryDto>> Countries()
    {
        await using MediaContext context = new();
        List<Country> countries = await context.Countries
            .ToListAsync();

        return countries.Select(country => new CountryDto
        {
            Name = country.EnglishName,
            Code = country.Iso31661
        }).ToList();
    }
}

public class LanguageDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("iso_639_1")] public string? Iso6391 { get; set; }
    [JsonProperty("english_name")] public string? EnglishName { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
}

public class CountryDto
{
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("code")] public string? Code { get; set; }
}