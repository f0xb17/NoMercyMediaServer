using System.Drawing;
using ColorThiefDotNet;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;

namespace NoMercy.Server.Logic;

public static class ImageLogic
{
    private static async Task<PaletteColors> CreateColorPalette(string path, string? size = null)
    {
        Bitmap bitmap;

        if (Uri.TryCreate(path, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) || size != null)
        {
            bitmap = await ImageClient.Download(path, size ?? "w300");
        }
        else
        {
            bitmap = new Bitmap(path);
        }

        var colorThief = new ColorThief();
        var dominantColor = colorThief.GetColor(bitmap);
        var palette = colorThief.GetPalette(bitmap, 6);

        return new PaletteColors
        {
            Dominant = dominantColor.Color.ToHexString(),
            Primary = palette[0].Color.ToHexString(),
            LightVibrant = palette[1].Color.ToHexString(),
            DarkVibrant = palette[2].Color.ToHexString(),
            LightMuted = palette[3].Color.ToHexString(),
            DarkMuted = palette[4].Color.ToHexString(),
        };
    }

    public class PaletteColors
    {
        [JsonProperty("dominant")] public required string Dominant { get; set; }
        [JsonProperty("primary")] public required string Primary { get; set; }
        [JsonProperty("lightVibrant")] public required string LightVibrant { get; set; }
        [JsonProperty("darkVibrant")] public required string DarkVibrant { get; set; }
        [JsonProperty("lightMuted")] public required string LightMuted { get; set; }
        [JsonProperty("darkMuted")] public required string DarkMuted { get; set; }
    }

    private static async Task<PaletteColors> ColorPaletteFromTmdbImage(string filePath)
    {
        PaletteColors palette = await CreateColorPalette(filePath, "w300");
        return palette;
    }

    public static async Task<String> GenerateColorPalette(
        string? posterPath = null,
        string? backdropPath = null,
        string? logoPath = null,
        string? profilePath = null,
        string? filePath = null,
        string? stillPath = null)
    {
        Dictionary<string, PaletteColors?> palette = new()
        {
            {
                "poster", posterPath != null
                    ? await ColorPaletteFromTmdbImage(posterPath)
                    : null
            },
            {
                "backdrop", backdropPath != null
                    ? await ColorPaletteFromTmdbImage(backdropPath)
                    : null
            },
            {
                "logo", logoPath != null
                    ? await ColorPaletteFromTmdbImage(logoPath)
                    : null
            },
            {
                "profile", profilePath != null
                    ? await ColorPaletteFromTmdbImage(profilePath)
                    : null
            },
            {
                "still", stillPath != null
                    ? await ColorPaletteFromTmdbImage(stillPath)
                    : null
            },
            {
                "image", filePath != null
                    ? await ColorPaletteFromTmdbImage(filePath)
                    : null
            },
        };

        return JsonConvert.SerializeObject(palette.Where(x => x.Value != null)
            .ToDictionary(x => x.Key, x => x.Value));
    }

    public static async Task GetPalette(string filePath)
    {
        await using MediaContext mediaContext = new MediaContext();
        var image = await mediaContext.Images
            .FirstOrDefaultAsync(e => e.FilePath == filePath);

        if (image is null) return;

        lock (image)
        {
            if (image is { _colorPalette: "" })
            {
                var palette = GenerateColorPalette(filePath: filePath).Result;
                image._colorPalette = palette;
                mediaContext.SaveChanges();
            }
        }
    }
}