using FFMpegCore;
using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using NoMercy.Providers.AcoustId.Client;
using NoMercy.Providers.AcoustId.Models;
using NoMercy.Server.Logic;
using NoMercy.Server.system;
using LogLevel = NoMercy.Helpers.LogLevel;

namespace NoMercy.Server.app.Jobs;

public class AddMusicJob : IShouldQueue
{    
    private readonly MediaContext _mediaContext = new();

    private readonly string? _libraryId;
    private readonly string _file;
    
    public AddMusicJob(string libraryId, string file)
    {
        _libraryId = libraryId;
        _file = file;
    }

    public async Task Handle()
    {
        Library? library;
        
        if (_libraryId is null)
        {
            library = await _mediaContext.Libraries
                .Where(f => f.Type == "music")
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstOrDefaultAsync();
        } 
        else {
            library = await _mediaContext.Libraries
                .Where(f => f.Id == Ulid.Parse(_libraryId))
                .Include(l => l.FolderLibraries)
                .ThenInclude(fl => fl.Folder)
                .FirstOrDefaultAsync();
        }
        
        using FingerprintClient fingerprintClient = new();
        Fingerprint? fingerPrint = await fingerprintClient.Lookup(_file);
        
        IMediaAnalysis mediaAnalysis = await FFProbe.AnalyseAsync(_file);
        int duration = (int)mediaAnalysis.Format.Duration.TotalSeconds;

        if (fingerPrint is null || library is null) return;
               
            foreach (var result in fingerPrint.Results)
            {
                // foreach (var recording in result.Recordings?.Where(r => r.Duration == duration) ?? [])
                foreach (var recording in result.Recordings ?? [])
                {
                    try
                    {
                        // if (recording.Title is null) continue;

                        MusicLogic music = new MusicLogic(recording.Id, library, _file, mediaAnalysis);
                        await music.Process();
                        
                        music.Dispose();
                    }
                    catch (Exception e)
                    {
                        Logger.App(e.InnerException?.Message ?? e.Message, LogLevel.Error);
                    }
                }
            }
         
        Logger.App($@"Music {_file}: Processed");
    }
}