using Newtonsoft.Json;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public record FilterRequest
{
    [JsonProperty("letter")] public string? Letter { get; set; } = "_";
}