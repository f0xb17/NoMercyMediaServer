using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music;
using NoMercy.Server.app.Http.Middleware;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Encoder Profiles")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/dashboard/encoderprofiles", Order = 10)]
public class EncoderController : Controller
{
    [HttpGet]
    public async Task<List<EncoderProfileDto>> Index()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var profiles = await mediaContext.EncoderProfiles.ToListAsync();

        List<EncoderProfileDto> encoderProfiles = [];

        foreach (var profile in profiles)
        {
            if (profile.Param == null) continue;

            var paramJson = JsonConvert.DeserializeObject<ParamsDto>(profile.Param);

            EncoderProfileDto profileDto = new()
            {
                Id = profile.Id.ToString(),
                Name = profile.Name,
                Container = profile.Container,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
                Params = paramJson is not null
                    ? new ParamsDto
                    {
                        Width = paramJson.Width,
                        Crf = paramJson.Crf,
                        Preset = paramJson.Preset,
                        Profile = paramJson.Profile,
                        Codec = paramJson.Codec,
                        Audio = paramJson.Audio
                    }
                    : null
            };
            encoderProfiles.Add(profileDto);
        }

        return encoderProfiles;
    }

    [HttpPost]
    public async Task<StatusResponseDto<EncoderProfile>> Create()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var libraries = await mediaContext.EncoderProfiles.CountAsync();

        EncoderProfile profile = new()
        {
            Name = $"Profile {libraries}",
            Container = "mp4",
            Param = JsonConvert.SerializeObject(new ParamsDto
            {
                Width = 1920,
                Crf = 23,
                Preset = "medium",
                Profile = "main",
                Codec = "libx264",
                Audio = "aac"
            })
        };

        return new StatusResponseDto<EncoderProfile>
        {
            Status = "ok",
            Data = profile
        };
    }

    [HttpDelete]
    public IActionResult Destroy()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }
}

public class EncoderProfileDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("container")] public string? Container { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("params")] public ParamsDto? Params { get; set; }
}

public class ContainerDto
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("val")] public string Val { get; set; }
}

public class ParamsDto
{
    [JsonProperty("video")] public int Width { get; set; }
    [JsonProperty("crf")] public int Crf { get; set; }
    [JsonProperty("preset")] public string Preset { get; set; }
    [JsonProperty("profile")] public string Profile { get; set; }
    [JsonProperty("codec")] public string Codec { get; set; }
    [JsonProperty("audio")] public string Audio { get; set; }
    [JsonProperty("tune")] public string Tune { get; set; }
    [JsonProperty("level")] public string Level { get; set; }
}