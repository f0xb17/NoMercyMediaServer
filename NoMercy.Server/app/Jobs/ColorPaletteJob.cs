using System.Globalization;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Server.Logic;
using NoMercy.Server.Logic.ImageLogic;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.app.Jobs;

[Serializable]
public class ColorPaletteJob : IShouldQueue, IDisposable, IAsyncDisposable
{
    public int? Id { get; set; }
    public string? FilePath { get; set; }
    public string? Type { get; set; }
    public string Model { get; set; }
    public string? Language { get; set; }

    public ColorPaletteJob()
    {
    }

    public ColorPaletteJob(long id, string model)
    {
        Id = (int)id;
        Model = model;
    }

    public ColorPaletteJob(long id, string type, string model)
    {
        Id = (int)id;
        Model = model;
        Type = type;
    }

    public ColorPaletteJob(string? filePath, string model, string? language = null)
    {
        FilePath = filePath;
        Model = model;
        Language = language;
    }

    public async Task Handle()
    {
        switch (Model)
        {
            case "image" when FilePath == null:
                await Task.CompletedTask;
                return;
            case "image":
                var shouldDownload = Language is null || Language is "en" || Language is "" ||
                                     Language == CultureInfo.CurrentCulture
                                         .TwoLetterISOLanguageName;

                await MovieDbImage.ColorPalette("image", FilePath, shouldDownload);
                break;
        }

        if (!Id.HasValue) return;

        switch (Model)
        {
            case "image" when FilePath == null:
                await Task.CompletedTask;
                return;
            case "collection":
                await CollectionLogic.Palette(Id.Value);
                break;
            case "tv":
                await TvShowLogic.Palette(Id.Value);
                break;
            case "person":
                await PersonLogic.Palette(Id.Value);
                break;
            case "season":
                await SeasonLogic.Palette(Id.Value);
                break;
            case "episode":
                await EpisodeLogic.Palette(Id.Value);
                break;
            case "movie":
                await MovieLogic.Palette(Id.Value);
                break;
            case "recommendation":
                switch (Type)
                {
                    case "movie":
                        await MovieLogic.RecommendationPalette(Id.Value);
                        break;
                    case "tv":
                        await TvShowLogic.RecommendationPalette(Id.Value);
                        break;
                    default:
                        Logger.Queue(@"Invalid model Type: " + Model + @" id: " + Id + @" type: " + Type,
                            LogLevel.Error);
                        break;
                }

                break;
            case "similar":
                switch (Type)
                {
                    case "movie":
                        await MovieLogic.SimilarPalette(Id.Value);
                        break;
                    case "tv":
                        await TvShowLogic.SimilarPalette(Id.Value);
                        break;
                    default:
                        Logger.Queue(@"Invalid model Type: " + Model + @" id: " + Id + @" type: " + Type,
                            LogLevel.Error);
                        break;
                }

                break;
            default:
                Logger.Queue(@"Invalid model Type: " + Model + @" id: " + Id + @" type: " + Type, LogLevel.Error);
                break;
        }
    }

    private static void ReleaseUnmanagedResources()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~ColorPaletteJob()
    {
        ReleaseUnmanagedResources();
    }
}