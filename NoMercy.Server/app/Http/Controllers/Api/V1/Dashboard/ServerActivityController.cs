#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Database;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Activity")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/activity", Order = 10)]
public class ServerActivityController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public async Task<ServerActivityDto[]> Index()
    {
        Guid userId = GetUserId();
        
        await using MediaContext mediaContext = new();
        ServerActivityDto[] activityDtos = mediaContext.ActivityLogs
            .OrderByDescending(x => x.CreatedAt)
            .Take(7)
            .Select(x => new ServerActivityDto
            {
                Id = x.Id,
                Type = x.Type,
                Time = x.Time,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                UserId = x.UserId,
                DeviceId = x.DeviceId,
                Device = x.Device.Name,
                User = x.User.Name
            })
            .ToArray();

        return activityDtos;
    }

    [HttpPost]
    public IActionResult Create()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpDelete]
    public IActionResult Destroy()
    {
        Guid userId = GetUserId();
        return Ok();
    }
}

public class ServerActivityDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("user_id")] public Guid UserId { get; set; }
    [JsonProperty("device_id")] public Ulid DeviceId { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("user")] public string User { get; set; }
}