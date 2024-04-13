using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Tasks")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/tasks", Order = 10)]
public class TasksController : Controller
{
    [NonAction]
    private Guid GetUserId()
    {
        return Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
    }

    [HttpGet]
    public TaskDto[] Index()
    {
        Guid userId = GetUserId();
        return
        [
            new TaskDto
            {
                Id = "pqiilkpnf8lmwrcxn0l8tngf",
                Title = "Scan media library",
                Value = 0,
                Type = "library",
                CreatedAt = DateTime.Parse("2024-01-25 09:26:56"),
                UpdatedAt = DateTime.Parse("2024-01-25 09:26:56")
            },
        ];
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

    [HttpDelete]
    public IActionResult Destroy()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpPost]
    [Route("pause")]
    public IActionResult PauseTask()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpPost]
    [Route("resume")]
    public IActionResult ResumeTask()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpGet]
    [Route("runners")]
    public IActionResult RunningTaskWorkers()
    {
        Guid userId = GetUserId();
        return Ok();
    }

    [HttpGet]
    [Route("queue")]
    public IActionResult EncoderQueue()
    {
        Guid userId = GetUserId();
        return Ok();
    }
}

public class TaskDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("value")] public int Value { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
}