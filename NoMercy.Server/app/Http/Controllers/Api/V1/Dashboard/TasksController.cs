using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music;
using NoMercy.Server.app.Http.Middleware;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Tasks")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/dashboard/tasks", Order = 10)]
public class TasksController : Controller
{
    [HttpGet]
    public TaskDto[] Index()
    {
        var userId = HttpContext.User.UserId();
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
            }
        ];
    }

    [HttpPost]
    public IActionResult Store()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPatch]
    public IActionResult Update()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
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

    [HttpPost]
    [Route("pause")]
    public IActionResult PauseTask()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    [Route("resume")]
    public IActionResult ResumeTask()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpGet]
    [Route("runners")]
    public IActionResult RunningTaskWorkers()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpGet]
    [Route("queue")]
    public IActionResult EncoderQueue()
    {
        var userId = HttpContext.User.UserId();

        return Ok(new PlaceholderResponse
        {
            Data = []
        });
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