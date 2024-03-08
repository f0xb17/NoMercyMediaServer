using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Devices")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/devices", Order = 10)]
public class DevicesController : Controller
{
    [HttpGet]
    public async Task<DevicesDto[]> Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        await using MediaContext mediaContext = new();
        var devices = await mediaContext.Devices
            .Include(device => device.ActivityLogs)
                // .ThenInclude(activityLog => activityLog.Device)
            .ToListAsync();
            
        var devicesDtos = devices
            .Select(x => new DevicesDto
            {
                Id = x.Id.ToString(),
                DeviceId = x.DeviceId,
                Browser = x.Browser,
                Os = x.Os,
                Device = x.CustomName,
                Type = x.Type,
                Name = x.Name,
                CustomName = x.CustomName,
                Version = x.Version,
                Ip = x.Ip,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                ActivityLogs = x.ActivityLogs
                    .Select(activityLog => new ActivityLogDto
                    {
                        Id = activityLog.Id,
                        Type = activityLog.Type,
                        Time = activityLog.Time,
                        CreatedAt = activityLog.CreatedAt,
                        UpdatedAt = activityLog.UpdatedAt,
                        UserId = activityLog.UserId,
                        DeviceId = activityLog.DeviceId.ToString(),
                    }), 
            });
        
        return devicesDtos.ToArray();
    }

    [HttpPost]
    public IActionResult Create()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Destroy()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }
}

public class DevicesDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
    [JsonProperty("browser")] public string Browser { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("custom_name")] public object? CustomName { get; set; }
    [JsonProperty("version")] public string Version { get; set; }
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("activity_logs")] public IEnumerable<ActivityLogDto> ActivityLogs { get; set; }
}

public class ActivityLogDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("user_id")] public Guid UserId { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
}