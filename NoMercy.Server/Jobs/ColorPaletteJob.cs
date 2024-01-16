using NoMercy.Helpers;
using NoMercy.Server.Logic;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.Jobs;

public class ColorPaletteJob : IShouldQueue
{
    private readonly int _id;
    private readonly string _model;

    public ColorPaletteJob(int id, string model)
    {
        // Logger.Queue($"Creating color palette job for {model} {id}", "info");
        _id = id;
        _model = model;
    }

    public async Task Handle()
    {
        // Logger.Queue($"Fetching color palette for {_model} {_id}", "info");
        if (_model == "movie")
        {
            // await MovieLogic.GetPalette(_id);
        }
        else if (_model == "tv")
        {
            await TvShowLogic.GetPalette(_id);
        }
        else if (_model == "person")
        {
            await PersonLogic.GetPalette(_id);
        }
        else if (_model == "season")
        {
            await SeasonLogic.GetPalette(_id);
        }
        else if (_model == "episode")
        {
            await EpisodeLogic.GetPalette(_id);
        }
        else
            Logger.Queue("Invalid model type", LogLevel.Error);
        
        await Task.CompletedTask;
    }
}