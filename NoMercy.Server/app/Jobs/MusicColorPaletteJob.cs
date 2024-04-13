using NoMercy.Helpers;
using NoMercy.Server.Logic;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.app.Jobs;

public class MusicColorPaletteJob : IShouldQueue
{
    private readonly string? _id;
    private readonly string _model;
    private readonly string? _filePath;
    private readonly bool _download;
    
    public MusicColorPaletteJob(string id, string model)
    {
        _id = id;
        _model = model;
    }
    
    public MusicColorPaletteJob(string filePath, string model, bool download)
    {
        _filePath = filePath;
        _model = model;
        _download = download;
    }

    public async Task Handle()
    {
        switch (_model)
        {
            case "image" when _filePath == null:
                await Task.CompletedTask;
                return;
            case "image":
                await ImageLogic.GetMusicPalette(_filePath, _download);
                break;
            case "artist":
                await MusicLogic.GetPalette(_id!, _model);
                break;
            case "album":
                await MusicLogic.GetPalette(_id!, _model);
                break;
            case "track":
                await MusicLogic.GetPalette(_id!, _model);
                break;
            default:
                Logger.Queue(@"Invalid model Type: " + _model + @" id: " + _id, LogLevel.Error);
                break;
        }
    }
}