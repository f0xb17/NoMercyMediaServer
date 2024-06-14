#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using AniDB.ResponseItems;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Helpers;
using NoMercy.Providers.AniDb.Clients;
using NoMercy.Server.app.Http.Controllers.Api.V1.DTO;
using NoMercy.Server.app.Http.Middleware;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard;

[ApiController]
[Tags("Dashboard Server Plugins")]
[ApiVersion("1")]
[Authorize]
[Route("api/v{Version:apiVersion}/dashboard/plugins", Order = 10)]
public class PluginController : Controller
{
    [HttpGet]
    public async Task<AniDBAnimeItem> Index()
    {
        var userId = HttpContext.User.UserId();

        var randomAnime = await AniDbRandomAnime.GetRandomAnime();

        return randomAnime;
    }

    [HttpGet]
    [Route("credentials")]
    public async Task<List<AniDbCredentialsResponse>> Credentials()
    {
        var userId = HttpContext.User.UserId();

        await using MediaContext context = new();
        var ownerId = TokenParamAuthMiddleware.Users.Where(u => u.Owner).Select(u => u.Id).FirstOrDefault();

        if (userId != ownerId) return [];

        var aniDb = CredentialManager.Credential("AniDb");

        if (aniDb == null) return [];

        return
        [
            new AniDbCredentialsResponse
            {
                Key = "AniDb",
                Username = aniDb.Username,
                ApiKey = aniDb.ApiKey
            }
        ];
    }

    [HttpPost]
    [Route("credentials")]
    public StatusResponseDto<string> Credentials([FromBody] AniDbCredentialsRequest request)
    {
        var userId = HttpContext.User.UserId();

        var ownerId = TokenParamAuthMiddleware.Users.Where(u => u.Owner).Select(u => u.Id).FirstOrDefault();

        if (userId != ownerId)
            return new StatusResponseDto<string>
            {
                Status = "error",
                Message = "You do not have permission to set credentials"
            };

        var aniDb = CredentialManager.Credential(request.Key);
        CredentialManager.SetCredentials(request.Key, request.Username, request.Password ?? aniDb?.Password ?? "",
            request.ApiKey);

        return new StatusResponseDto<string>
        {
            Status = "ok",
            Message = "Credentials set successfully for {0}",
            Args = [request.Key]
        };
    }
}

public class AniDbCredentialsRequest
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("password")] public string? Password { get; set; }
    [JsonProperty("api_key")] public string ApiKey { get; set; }
}

public class AniDbCredentialsResponse
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("api_key")] public string? ApiKey { get; set; }
}