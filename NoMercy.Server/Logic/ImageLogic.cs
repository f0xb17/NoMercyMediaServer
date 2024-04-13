using System.Drawing;
using ColorThiefDotNet;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Providers.TMDB.Client;

namespace NoMercy.Server.Logic;

public static class ImageLogic
{
    private static async Task<PaletteColors> CreateColorPalette(string path, string? size = null, bool? download = false)
    {
        Bitmap? bitmap;

        if (Uri.TryCreate(path, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) && download is false)
        {
            bitmap = new Bitmap(path);
        }
        else
        {
            if (size is not null)
            {
                bitmap = await ImageClient.Download(path, size, download);
            }
            else
            {
                bitmap = await ImageClient.Download(path, "music");
            }
        }
        
        if (bitmap is null) return new PaletteColors
        {
            Dominant = null,
            Primary = null,
            LightVibrant = null,
            DarkVibrant = null,
            LightMuted = null,
            DarkMuted = null
        };
        
        var colorThief = new ColorThief();
        var dominantColor = colorThief.GetColor(bitmap, 10 ,false);
        var palette = colorThief.GetPalette(bitmap, 6, 10 ,false);

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

    private static async Task<PaletteColors> ColorPaletteFromTmdbImage(string filePath, bool download = true)
    {
        PaletteColors palette = await CreateColorPalette(filePath, "original", download);
        return palette;
    }
    private static async Task<PaletteColors> ColorPaletteFromCoverArchiveImage(string coverPath, bool download = true)
    {
        PaletteColors palette = await CreateColorPalette(coverPath, null, download);
        return palette;
    }

    public static async Task<String> GenerateColorPalette(
        string? posterPath = null,
        string? backdropPath = null,
        string? logoPath = null,
        string? profilePath = null,
        string? filePath = null,
        string? coverPath = null,
        string? stillPath = null,
        bool download = true
        )
    {
        Dictionary<string, PaletteColors?> palette = new()
        {
            {
                "poster", posterPath != null
                    ? await ColorPaletteFromTmdbImage(posterPath, download)
                    : null
            },
            {
                "backdrop", backdropPath != null
                    ? await ColorPaletteFromTmdbImage(backdropPath, download)
                    : null
            },
            {
                "logo", logoPath != null
                    ? await ColorPaletteFromTmdbImage(logoPath, download)
                    : null
            },
            {
                "profile", profilePath != null
                    ? await ColorPaletteFromTmdbImage(profilePath, download)
                    : null
            },
            {
                "still", stillPath != null
                    ? await ColorPaletteFromTmdbImage(stillPath, download)
                    : null
            },
            {
                "image", filePath != null
                    ? await ColorPaletteFromTmdbImage(filePath, download)
                    : null
            },
            {
                "cover", coverPath != null
                    ? await ColorPaletteFromCoverArchiveImage(coverPath, download)
                    : null
            },
        };

        return JsonConvert.SerializeObject(palette.Where(x => x.Value != null)
            .ToDictionary(x => x.Key, x => x.Value));
    }

    public static async Task GetPalette(string filePath, bool download = true)
    {
        await using MediaContext mediaContext = new MediaContext();
        var image = await mediaContext.Images
            .FirstOrDefaultAsync(e => e.FilePath == filePath);

        if (image is null) return;

        lock (image)
        {
            if (image is { _colorPalette: "" })
            {
                var palette = GenerateColorPalette(filePath: filePath, download: download).Result;
                image._colorPalette = palette;
                mediaContext.SaveChanges();
            }
        }
    }
    
    public static async Task GetMusicPalette(string filePath, bool download = true)
    {
        await using MediaContext mediaContext = new MediaContext();
        var image = await mediaContext.Images
            .FirstOrDefaultAsync(e => e.FilePath == filePath);

        if (image is null) return;

        lock (image)
        {
            if (image is { _colorPalette: "" })
            {
                var palette = GenerateColorPalette(coverPath: filePath, download: download).Result;
                image._colorPalette = palette;
                mediaContext.SaveChanges();
            }
        }
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

}