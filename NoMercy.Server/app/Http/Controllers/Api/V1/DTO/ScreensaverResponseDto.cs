using Newtonsoft.Json;
using NoMercy.Database;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Http.Controllers.Api.V1.DTO;

public class ScreensaverResponseDto
{
    [JsonProperty("aspectRatio")]
    public double AspectRatio { get; set; }

    [JsonProperty("src")]
    public string Src { get; set; }

    [JsonProperty("colorPalette", NullValueHandling = NullValueHandling.Ignore)]
    public IColorPalettes ColorPaletteDto { get; set; }

    [JsonProperty("meta")]
    public MetaDto MetaDto { get; set; }
}

public class MetaDto
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("logo")]
    public LogoDto LogoDto { get; set; }
}

public class LogoDto
{
    [JsonProperty("aspectRatio")]
    public double AspectRatio { get; set; }

    [JsonProperty("src")]
    public string Src { get; set; }
}