using Newtonsoft.Json;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard.DTO;

public record PermissionsResponseDto
{
    [JsonProperty("edit")] public bool Edit { get; set; }
}