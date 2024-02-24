using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Activity")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/activity", Order = 10)]
public class ServerActivityController : Controller
{
    [HttpGet]
    public ServerActivityDto[] Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        return
        [
            new ServerActivityDto
            {
                Id = 13047,
                Type = "Connected",
                Time = 1708266313245,
                CreatedAt = "2024-02-18 14:25:13",
                UpdatedAt = "2024-02-18 14:25:13",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13046,
                Type = "Connected",
                Time = 1708266305456,
                CreatedAt = "2024-02-18 14:25:05",
                UpdatedAt = "2024-02-18 14:25:05",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13045,
                Type = "Disconnected",
                Time = 1708266298133,
                CreatedAt = "2024-02-18 14:24:58",
                UpdatedAt = "2024-02-18 14:24:58",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13044,
                Type = "Disconnected",
                Time = 1708266288932,
                CreatedAt = "2024-02-18 14:24:48",
                UpdatedAt = "2024-02-18 14:24:48",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13043,
                Type = "Connected",
                Time = 1708266281348,
                CreatedAt = "2024-02-18 14:24:41",
                UpdatedAt = "2024-02-18 14:24:41",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13042,
                Type = "Connected",
                Time = 1708263961324,
                CreatedAt = "2024-02-18 13:46:01",
                UpdatedAt = "2024-02-18 13:46:01",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
            new ServerActivityDto
            {
                Id = 13041,
                Type = "Connected",
                Time = 1708263274323,
                CreatedAt = "2024-02-18 13:34:34",
                UpdatedAt = "2024-02-18 13:34:34",
                UserId = "6aa35c70-7136-44f3-baba-e1d464433426",
                DeviceId = "abf975cc-1c9f-37bd-bd7b-14470174e27b",
                Device = "Vue Dev Brave",
                User = "Stoney Eagle"
            },
        ];
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

public class ServerActivityDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("time")] public long Time { get; set; }
    [JsonProperty("created_at")] public string CreatedAt { get; set; }
    [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    [JsonProperty("user_id")] public string UserId { get; set; }
    [JsonProperty("device_id")] public string DeviceId { get; set; }
    [JsonProperty("device")] public string Device { get; set; }
    [JsonProperty("user")] public string User { get; set; }
}