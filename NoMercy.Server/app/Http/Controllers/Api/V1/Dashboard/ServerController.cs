using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Helpers;
using NoMercy.Queue.system;
using NoMercy.Server.app.Helper;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using LogLevel = NoMercy.Helpers.LogLevel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Management")]
[ApiVersion("1")]
[Authorize, Route("api/v{Version:apiVersion}/dashboard/server", Order = 10)]
public class ServerController : Controller
{
    private IHostApplicationLifetime ApplicationLifetime { get; }

    public ServerController(IHostApplicationLifetime appLifetime)
    {
        ApplicationLifetime = appLifetime;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return Ok();
    }

    [HttpPost]
    [Route("start")]
    public IActionResult StartServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("stop")]
    public IActionResult StopServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("restart")]
    public IActionResult RestartServer()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        // ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("shutdown")]
    public IActionResult Shutdown()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        ApplicationLifetime.StopApplication();
        return Content("Done");
    }

    [HttpPost]
    [Route("loglevel")]
    public IActionResult LogLevel(LogLevel level)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        Logger.SetLogLevel(level);

        return Content("Log level set to " + level);
    }

    [HttpPost]
    [Route("/api/v{Version:apiVersion}/dashboard/directorytree")]
    public StatusResponseDto<List<DirectoryTreeDto>> DirectoryTree([FromBody] PathRequest request)
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        string? path = request.Path ?? request.Folder;

        List<DirectoryTreeDto> array = [];

        if (string.IsNullOrEmpty(path) || path == "/")
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform
                    .Windows))
            {
                DriveInfo[] driveInfo = DriveInfo.GetDrives();
                array = driveInfo.Select(d => CreateDirectoryTreeDto("", d.RootDirectory.ToString())).ToList();

                return new StatusResponseDto<List<DirectoryTreeDto>>
                {
                    Status = "ok",
                    Data = array
                };
            }

            path = "/";
        }

        Logger.App(path);

        if (!Directory.Exists(path))
        {
            return new StatusResponseDto<List<DirectoryTreeDto>>
            {
                Status = "ok",
                Data = array
            };
        }

        try
        {
            var directories = Directory.GetDirectories(path);
            array = directories.Select(d => CreateDirectoryTreeDto(path, d)).ToList();
        }
        catch (Exception ex)
        {
            return new StatusResponseDto<List<DirectoryTreeDto>>
            {
                Status = "error",
                Message = ex.Message
            };
        }

        return new StatusResponseDto<List<DirectoryTreeDto>>
        {
            Status = "ok",
            Data = array
        };
    }

    private DirectoryTreeDto CreateDirectoryTreeDto(string parent, string path)
    {
        string fullPath = Path.Combine(parent, path);

        FileInfo fileInfo = new FileInfo(fullPath);

        string type = fileInfo.Attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";

        return new DirectoryTreeDto
        {
            Path = string.IsNullOrEmpty(fileInfo.Name)
                ? path
                : fileInfo.Name,
            Parent = Path.Combine(fullPath, ".."),
            FullPath = fullPath,
            Mode = (int)fileInfo.Attributes,
            Size = type == "file" ? int.Parse(fileInfo.Length.ToString()) : null,
            Type = type
        };
    }

    [HttpGet]
    [Route("info")]
    public ServerInfoDto ServerInfo()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        
        return new ServerInfoDto
        {
            Server = SystemInfo.DeviceName,
            Cpu = SystemInfo.Cpu,
            Os = SystemInfo.Platform.ToTitleCase(),
            Arch = SystemInfo.Architecture,
            Version = SystemInfo.Version,
            BootTime = SystemInfo.StartTime
        };
    }

    [HttpGet]
    [Route("paths")]
    public ServerPathsDto[] ServerPaths()
    {
        return
        [
            new ServerPathsDto
            {
                Key = "Cache",
                Value = AppFiles.CachePath
            },
            new ServerPathsDto
            {
                Key = "Logs",
                Value = AppFiles.LogPath
            },
            new ServerPathsDto
            {
                Key = "Metadata",
                Value = AppFiles.MetadataPath
            },
            new ServerPathsDto
            {
                Key = "Transcodes",
                Value = AppFiles.TranscodePath
            },
            new ServerPathsDto
            {
                Key = "Configs",
                Value = AppFiles.ConfigPath
            }
        ];
    }

    [HttpGet]
    [Route("/files/${depth:int}/${path:required}")]
    public async Task<List<MediaFolder>?> Files(string path, int depth)
    {
        Logger.App($@"Files: {path}");
        MediaScan mediaScan = new();

        List<MediaFolder>? folders = await mediaScan
            .EnableFileListing()
            .Process(path, depth);

        mediaScan.Dispose();

        return folders;
    }

    [HttpPatch]
    [Route("workers/{worker}/{count:int:min(0)}")]
    public async Task<IActionResult> UpdateWorkers(string worker, int count)
    {
        if (await QueueRunner.SetWorkerCount(worker, count))
        {
            return Ok($"{worker} worker count set to {count}");
        }

        return BadRequest($"{worker} worker count could not be set to {count}");
    }
}

public class ServerInfoDto
{
    [JsonProperty("server")] public string Server { get; set; }
    [JsonProperty("cpu")] public string? Cpu { get; set; }
    [JsonProperty("os")] public string Os { get; set; }
    [JsonProperty("arch")] public string Arch { get; set; }
    [JsonProperty("version")] public string? Version { get; set; }
    [JsonProperty("bootTime")] public DateTime BootTime { get; set; }
}

public class ServerPathsDto
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("value")] public string Value { get; set; }
}

public class DirectoryRequest
{
    [JsonProperty("path")] public string Path { get; set; }
}

public class PathRequest
{
    [JsonProperty("path")] public string? Path { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
}

public class DirectoryTreeDto
{
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("mode")] public int Mode { get; set; }
    [JsonProperty("size")] public int? Size { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("parent")] public string Parent { get; set; }
    [JsonProperty("full_path")] public string FullPath { get; set; }
}