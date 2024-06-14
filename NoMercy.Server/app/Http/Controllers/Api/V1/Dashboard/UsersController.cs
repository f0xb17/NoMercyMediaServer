#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Controllers.Api.V1.Music;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Users")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/dashboard/users", Order = 10)]
public class UsersController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var userId = HttpContext.User.UserId();


        return Ok(new PlaceholderResponse
        {
            Data = []
        });
    }

    [HttpPost]
    public async Task<StatusResponseDto<string>> Store([FromBody] UserRequest request)
    {
        var userId = HttpContext.User.UserId();

        Logger.Access($"User {userId} attempting to create a user");

        await using MediaContext mediaContext = new();

        var hasPermission = mediaContext.Users.FirstOrDefault(user => user.Id == userId);

        if (hasPermission is null || hasPermission.Owner is false)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "You do not have permission to create a user"
            };

        var user = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .FirstOrDefaultAsync(user => user.Id == request.Id);

        if (user != null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "User already exists"
            };

        User newUser = new()
        {
            Id = request.Id,
            Email = request.Email,
            Name = request.Name,
            Allowed = true,
            AudioTranscoding = false,
            VideoTranscoding = false,
            NoTranscoding = true
        };

        mediaContext.Users.Add(newUser);

        await mediaContext.SaveChangesAsync();

        var createdUser = mediaContext.Users
            .Include(u => u.LibraryUser)
            .First(u => u.Id == newUser.Id);

        TokenParamAuthMiddleware.Users.Add(createdUser);

        return new StatusResponseDto<string>
        {
            Status = "success",
            Message = "User {0} created successfully",
            Data = createdUser.Name
        };
    }

    [HttpPatch("{id:guid}")]
    public async Task<StatusResponseDto<string>> Update(Guid id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var user = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "User not found"
            };

        // TODO Implement user update

        return new StatusResponseDto<string>
        {
            Status = "success",
            Message = "User updated"
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<StatusResponseDto<string>> Destroy(Guid id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var user = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "User not found"
            };

        mediaContext.Users.Remove(user);
        await mediaContext.SaveChangesAsync();

        TokenParamAuthMiddleware.Users.Remove(user);

        return new StatusResponseDto<string>
        {
            Status = "success",
            Message = "User deleted"
        };
    }

    [HttpGet]
    [Route("permissions")]
    public async Task<PermissionsResponseDto> UserPermissions()
    {
        await using MediaContext mediaContext = new();

        var permissions = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .ThenInclude(libraryUser => libraryUser.Library)
            .ToListAsync();

        return new PermissionsResponseDto
        {
            Data = permissions.Select(user => new PermissionsResponseItemDto(user)) ?? []
        };
    }

    [HttpPatch]
    [Route("permissions/{id:guid}")]
    public async Task<StatusResponseDto<string>> UpdateUserPermissions(Guid id)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var user = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "User not found"
            };

        // TODO Implement user permissions

        return new StatusResponseDto<string>
        {
            Status = "success",
            Message = "User permissions updated"
        };
    }

    [HttpPatch("notificationsettings")]
    public async Task<StatusResponseDto<string>> NotificationSettings([FromBody] object request)
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext mediaContext = new();
        var user = await mediaContext.Users
            .Include(user => user.LibraryUser)
            .Include(user => user.NotificationUser)
            .ThenInclude(notificationUser => notificationUser.Notification)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "User not found"
            };

        // TODO Implement notification settings

        return new StatusResponseDto<string>
        {
            Status = "success",
            Message = "Notification settings updated"
        };
    }
}

public class PermissionsResponseDto
{
    [JsonProperty("data")] public IEnumerable<PermissionsResponseItemDto> Data { get; set; }
}

public class PermissionsResponseItemDto : User
{
    [JsonProperty("library_user")] public new LibraryUserDto[] LibraryUser { get; set; }
    [JsonProperty("libraries")] public Ulid[] Libraries { get; set; }

    public PermissionsResponseItemDto(User user)
    {
        Id = user.Id;
        Email = user.Email;
        Manage = user.Manage;
        Owner = user.Owner;
        Name = user.Name;
        Allowed = user.Allowed;
        AudioTranscoding = user.AudioTranscoding;
        VideoTranscoding = user.VideoTranscoding;
        NoTranscoding = user.NoTranscoding;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;

        LibraryUser = user.LibraryUser
            .Select(libraryUser => new LibraryUserDto
            {
                LibraryId = libraryUser.LibraryId,
                UserId = libraryUser.UserId
            })
            .ToArray();

        Libraries = user.LibraryUser
            .Select(libraryUser => libraryUser.Library.Id)
            .ToArray();
    }
}

public class LibraryUserDto
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("UserId")] public Guid UserId { get; set; }
}

public class UserRequest
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("manage")] public bool Manage { get; set; }
    [JsonProperty("owner")] public bool Owner { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("allowed")] public bool Allowed { get; set; }
    [JsonProperty("audio_transcoding")] public bool AudioTranscoding { get; set; }
    [JsonProperty("video_transcoding")] public bool VideoTranscoding { get; set; }
    [JsonProperty("no_transcoding")] public bool NoTranscoding { get; set; }
}