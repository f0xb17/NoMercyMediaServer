using Newtonsoft.Json;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard.DTO;

public class PermissionsResponseDto
{
    [JsonProperty("edit")] public bool Edit { get; set; }
}