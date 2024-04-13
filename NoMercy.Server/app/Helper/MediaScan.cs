#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using FFMpegCore;
using MovieFileLibrary;
using Newtonsoft.Json;
using NoMercy.Helpers;

namespace NoMercy.Server.app.Helper;

public class MovieFileExtend
{
    public List<Guid> ArtistIds { get; set; }
    public Guid? AcousticId { get; set; }
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Year { get; set; }
    public bool IsSeries { get; set; }
    public int? Season { get; set; }
    public int? Episode { get; set; }
    public bool IsSuccess { get; set; }
    public string FilePath { get; set; }
    public string FileExtension { get; set; }
}

public class MediaFolder
{
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; }
    public ConcurrentBag<MediaFile>? Files { get; set; }
    public ConcurrentBag<MediaFolder>? SubFolders { get; set; }
    public MovieFileExtend? Parsed { get; set; }
}

public class MediaFile
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Extension { get; set; }
    public int Size { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; }
    public MovieFileExtend? Parsed { get; set; }
    public IMediaAnalysis? FFprobe { get; set; }
    
}

public class MediaScan
{
    private readonly MovieDetector _movieDetector = new();
    
    private bool _fileListingEnabled;
    private bool _regexFilterEnabled = true;
    private readonly Regex _folderNameRegex = new(@"video_.*|audio_.*|subtitles|scans|cds.*|ost|album|music|original|fonts|thumbs|metadata|NCED|NCOP|\s\(\d\)\.|~", RegexOptions.IgnoreCase);

    public MediaScan()
    {
        GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(AppFiles.BinariesPath, "ffmpeg"));
    }
    
    public MediaScan EnableFileListing()
    {
        _fileListingEnabled = true;

        return this;
    }
    
    public MediaScan DisableRegexFilter()
    {
        _regexFilterEnabled = false;

        return this;
    }
    
    public Task<ConcurrentBag<MediaFolder>> Process(string rootFolder, int depth = 0)
    {
        return !_fileListingEnabled 
            ? Task.Run(() => ScanFoldersOnly(rootFolder, depth))
            : Task.Run(() => ScanFolder(rootFolder, depth));
    }

    private ConcurrentBag<MediaFolder> ScanFolder(string folderPath, int depth)
    {
        ConcurrentBag<MediaFolder> folders = [];
        
        if (depth < 0) return folders;
        
        ConcurrentBag<MediaFile> files = GetFiles(folderPath);
        
        MovieFile movieFile1 = _movieDetector.GetInfo(folderPath);
        movieFile1.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})").Match(folderPath)
            .Value;
        
        folders.Add(new MediaFolder
        {
            Name = Path.GetFileName(folderPath),
            Path = folderPath,
            Created = Directory.GetCreationTime(folderPath),
            Modified = Directory.GetLastWriteTime(folderPath),
            Accessed = Directory.GetLastAccessTime(folderPath),
            Type = "folder",
            Parsed = new MovieFileExtend
            {
                Title = movieFile1.Title,
                Year = movieFile1.Year,
                FilePath = movieFile1.FilePath,
            },
            
            Files = files.Count > 0
                ? files
                : null,
        });

        try
        {
            var directories = Directory.GetDirectories(folderPath).OrderBy(f => f);

            Parallel.ForEach(directories, (directory, _) =>
            {
                var folderName = Path.GetFileName(directory);

                if (_regexFilterEnabled && _folderNameRegex.IsMatch(folderName) || depth == 0)
                {
                    files.Add(new MediaFile
                    {
                        Name = folderName,
                        Path = directory,
                        Created = Directory.GetCreationTime(directory),
                        Modified = Directory.GetLastWriteTime(directory),
                        Accessed = Directory.GetLastAccessTime(directory),
                        Type = "folder",
                    });
                    
                    return;
                }

                ConcurrentBag<MediaFile> files2 = depth - 1 > 0 ? GetFiles(directory) : [];

                MovieFile movieFile = _movieDetector.GetInfo(directory);
                movieFile.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})")
                    .Match(directory).Value;

                folders.Add(new MediaFolder
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",
                    Parsed = new MovieFileExtend
                    {
                        Title = movieFile.Title,
                        Year = movieFile.Year,
                        FilePath = movieFile.FilePath,
                    },

                    Files = files2.Count > 0
                        ? files2
                        : null,

                    SubFolders = depth - 1 > 0
                        ? ScanFolder(directory, depth - 1)
                        : null,
                });
            });
            
            ConcurrentBag<MediaFolder> response = new ConcurrentBag<MediaFolder>(folders
                .Where(f => f.Name is not "")
                .OrderByDescending(f => f.Name));
            
            return response;
        }
        catch (Exception _)
        {
            return [];
        }
    }

    private ConcurrentBag<MediaFolder> ScanFoldersOnly(string folderPath, int depth)
    {
        if (depth < 0) return [];

        try
        {
            ConcurrentBag<MediaFolder> folders = [];
            
            var directories = Directory.GetDirectories(folderPath).OrderBy(f => f);

            Parallel.ForEach(directories, (directory, _) =>
            {
                Logger.App($"Scanning {directory}");

                var folderName = Path.GetFileName(directory);

                if (_regexFilterEnabled && _folderNameRegex.IsMatch(folderName))
                {
                    folders.Add(new MediaFolder
                    {
                        Name = folderName,
                        Path = directory,
                        Created = Directory.GetCreationTime(directory),
                        Modified = Directory.GetLastWriteTime(directory),
                        Accessed = Directory.GetLastAccessTime(directory),
                        Type = "folder",
                    });
                    
                    return;
                }

                MovieFile movieFile = _movieDetector.GetInfo(directory);

                movieFile.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})")
                    .Match(directory).Value;

                folders.Add(new MediaFolder
                {
                    Name = folderName,
                    Path = directory,
                    Created = Directory.GetCreationTime(directory),
                    Modified = Directory.GetLastWriteTime(directory),
                    Accessed = Directory.GetLastAccessTime(directory),
                    Type = "folder",

                    Parsed = new MovieFileExtend
                    {
                        Title = movieFile.Title,
                        Year = movieFile.Year,
                        FilePath = movieFile.FilePath,
                    },

                    SubFolders = depth - 1 > 0
                        ? ScanFoldersOnly(directory, depth - 1)
                        : null,
                });
            });
            
            ConcurrentBag<MediaFolder> response = new ConcurrentBag<MediaFolder>(folders
                .Where(f => f.Name is not "")
                .OrderByDescending(f => f.Name));
            
            return response;
        }
        catch (Exception e)
        {
            return [];
        }
    }
    
    private ConcurrentBag<MediaFile> GetFiles(string folderPath)
    {
        try
        {
            ConcurrentBag<MediaFile> files = [];

            Parallel.ForEach(Directory.GetFiles(folderPath), (file, _) =>
            {
                var extension = Path.GetExtension(file).ToLower();

                bool isVideoFile = extension is ".mp4" or ".avi" or ".mkv" or ".m3u8";
                bool isAudioFile = extension is ".mp3" or ".flac" or ".wav" or ".m4a";
                bool isSubtitleFile = extension is ".srt" or ".vtt" or ".ass" or ".sub";

                if (!isVideoFile && !isAudioFile && !isSubtitleFile) return;

                MovieFile? movieFile = isVideoFile || isAudioFile ? _movieDetector.GetInfo(file) : null;
                AnimeInfo animeInfo = AnimeParser.ParseAnimeFilename(file);

                if (movieFile is not null)
                {
                    movieFile.Year ??= new Regex(@"(1(8|9)|20)\d{2}(?!p|i|(1(8|9)|20)\d{2}|\]|\W(1(8|9)|20)\d{2})")
                        .Match(file).Value;
                    movieFile.Title ??= animeInfo.Name;
                    movieFile.Season ??= animeInfo.Season;
                    movieFile.Episode ??= animeInfo.Episode;
                }

                MovieFileExtend movieFileExtend = new MovieFileExtend
                {
                    FilePath = movieFile?.FilePath ?? file,
                    Episode = movieFile?.Episode,
                    Year = movieFile?.Year,
                    Season = movieFile?.Season,
                    Title = movieFile?.Title,
                    IsSeries = movieFile?.IsSeries ?? false,
                    IsSuccess = movieFile?.IsSuccess ?? false,
                };

                IMediaAnalysis? ffprobe;
                try
                {
                    ffprobe = isVideoFile
                        ? FFProbe.Analyse(file)
                        : null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                MediaFile res = new MediaFile
                {
                    Name = Path.GetFileName(file),
                    Path = file,
                    Extension = extension,
                    Size = (int)new FileInfo(file).Length,
                    Created = File.GetCreationTime(file),
                    Modified = File.GetLastWriteTime(file),
                    Accessed = File.GetLastAccessTime(file),
                    Type = "file",

                    Parsed = movieFileExtend,
                    FFprobe = ffprobe
                };

                files.Add(res);
            });
            
            ConcurrentBag<MediaFile> response = new ConcurrentBag<MediaFile>(files
                .Where(f => f.Name is not "")
                .OrderByDescending(f => f.Name));

            return response;

        }
        catch (Exception _)
        {
            return [];
        }
    }

     public void Dispose()
     {
         GC.Collect();
         GC.WaitForFullGCComplete();
     }
}