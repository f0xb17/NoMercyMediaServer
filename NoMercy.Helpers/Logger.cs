using System.Drawing;
using System.Text;
// ReSharper disable All
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Helpers;
using Pastel;

using System;
using Microsoft.Extensions.Logging;

public enum LogLevel
{
    Verbose,
    Debug,
    Info,
    Warning,
    Error
}
public static class Logger
{
    private static LogLevel _maxLogLevel = LogLevel.Verbose;
    
    static Logger()
    {
        // MediaContext mediaContext = new MediaContext();
        //
        // Configuration? config = mediaContext.Configuration
        //     .FirstOrDefault(c => c.Key == "logLevel");
        //
        // _maxLogLevel = (LogLevel) Enum.Parse(typeof(LogLevel), config?.Value ?? "Info");
    }
    
    public static void SetLogLevel(LogLevel level)
    {
        _maxLogLevel = level;
    }
    
    private static bool ShouldLog(LogLevel level)
    {
        return level >= _maxLogLevel;
    }

    private static readonly Dictionary<string, Color>? LogColors = new()
    {
        { "debug", Color.DarkGray },
        { "info", Color.White },
        { "warning", Color.Yellow },
        { "error", Color.Red },
        
        { "system", Color.DimGray },
        
        { "app", Color.MediumPurple },
        { "access", Color.MediumPurple },
        { "configuration", Color.MediumPurple },
        
        { "auth", Color.CornflowerBlue },
        { "register", Color.CornflowerBlue },
        { "setup", Color.CornflowerBlue },
        { "certificate", Color.CornflowerBlue },
        
        { "queue", Color.Salmon },
        { "encoder", Color.Salmon },
        { "ripper", Color.Salmon },
        
        { "http", Color.Orange },
        { "notify", Color.Orange },
        { "ping", Color.Orange },
        { "socket", Color.Orange},
        
        
        { "fanart", Color.DodgerBlue },
        { "fingerprint", Color.DodgerBlue },
        { "moviedb", Color.DodgerBlue },
        { "musicbrainz", Color.DodgerBlue },
        { "tvdb", Color.DodgerBlue },
        { "youtube", Color.DodgerBlue },
        { "openSubs", Color.DodgerBlue },
        
        { "qbittorrent", Color.Olive },
        { "transmission", Color.Olive },
        { "rutorrent", Color.Olive },
        { "sabnzbd", Color.Olive },
        
        { "discord", Color.Lime },
        { "twitter", Color.Lime },
        { "whatsapp", Color.Lime },
        { "telegram", Color.Lime },
        { "webhook", Color.Lime },
        
        
        // { "deluge", Color.White },
        // { "emby", Color.White },
        // { "jackett", Color.White },
        // { "jellyfin", Color.White },
        // { "kodi", Color.White },
        // { "lidarr", Color.White },
        // { "ombi", Color.White },
        // { "plex", Color.White },
        // { "radarr", Color.White },
        // { "sickchill", Color.White },
        // { "sickrage", Color.White },
        // { "sickgear", Color.White},
        // { "sonarr", Color.White },
        // { "tautulli", Color.White },
        // { "tivimate", Color.White },
        // { "trakt", Color.White },
        // { "tvmaze", Color.White },
        // { "usenet", Color.White },
        // { "xteve", Color.White },
    };

    private static readonly Dictionary<string, string> Capitalize = new()
    {
        { "debug", "DEBUG" },
        { "info", "INFO" },
        { "warning", "WARNING" },
        { "error", "ERROR" },
        { "system", "system" },
        { "app", "App" },
        { "access", "Access" },
        { "configuration", "Configuration" },
        { "auth", "Auth" },
        { "register", "Register" },
        { "setup", "Setup" },
        { "certificate", "Certificate" },
        { "queue", "Queue" },
        { "encoder", "Encoder" },
        { "ripper", "Ripper" },
        { "http", "Http" },
        { "notify", "Notify" },
        { "ping", "Ping" },
        { "socket", "Socket" },
        { "fanart", "FanArt" },
        { "fingerprint", "Fingerprint" },
        { "moviedb", "MovieDb" },
        { "musicbrainz", "MusicBrainz" },
        { "tvdb", "Tvdb" },
        { "youtube", "Youtube" },
        { "opensubs", "OpenSubs" },
        { "qbittorrent", "QBitTorrent" },
        { "transmission", "Transmission" },
        { "ruTorrent", "RuTorrent" },
        { "sabnzbd", "SabNzbd" },
        { "discord", "Discord" },
        { "twitter", "Twitter" },
        { "whatsapp", "Whatsapp" },
        { "telegram", "Telegram" },
        { "webhook", "Webhook" },
        
        { "deluge", "Deluge" },
        { "emby", "Emby" },
        { "jackett", "Jackett" },
        { "jellyfin", "Jellyfin" },
        { "kodi", "Kodi" },
        { "lidarr", "Lidarr" },
        { "ombi", "Ombi" },
        { "plex", "Plex" },
        { "radarr", "Radarr" },
        { "sickchill", "SickChill" },
        { "sickrage", "SickRage" },
        { "sickgear", "SickGear" },
        { "sonarr", "sonarr" },
        { "trakt", "Trakt" },
        { "tvmaze", "TvMaze" },
        { "usenet", "Usenet" },
        { "xteve", "Xteve" },
    };

    private static Color GetColor(string type)
    {
        return LogColors?[type] ?? Color.White;
    }
 
    public static T Log<T>(this T self, string type = "server") where T: class
    {
        return Log<T>(self, LogLevel.Debug, type);
    }

    private static T Log<T>(this T self, LogLevel level = LogLevel.Debug, string type = "server") where T: class
    {
        Log(level, self, type);
        return self;
    }
    
    public static T Log<T>(this T self) where T: class
    {
        Log(LogLevel.Debug, self);
        return self;
    }
    
    private static void Log<T>(LogLevel level, T message, string type = "server") where T: class
    {
        Log(level, message.ToJson(), type);
    }
    
    private static void Log(LogLevel level, string message, string type = "server")
    {
        if (!ShouldLog(level))
            return;
        
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Pastel(Color.DimGray);
        string logEntry = $"{Spacer(Capitalize[type] ?? type, 12).Pastel(GetColor(type))}: {timestamp} {message}";
        Console.WriteLine(logEntry);
    }
    
    private static void Log(LogLevel level, int message, string type = "server")
    {
        if (!ShouldLog(level))
            return;
        
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Pastel(Color.DimGray);
        string logEntry = $"{Spacer(Capitalize[type] ?? type, 12).Pastel(GetColor(type))}: {timestamp} {message}";
        Console.WriteLine(logEntry);
    }
    
    public static void Debug(string message) => Log(LogLevel.Debug, message, "debug");
    public static void Debug<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message);
    public static void Info(string message) => Log(LogLevel.Info, message, "info");
    public static void Info<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message);
    public static void Warning(string message) => Log(LogLevel.Warning, message, "warning");
    public static void Warning<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message);
    public static void Error(string message) => Log(LogLevel.Error, message, "error");
    public static void Error<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message);
    
    public static void System(string message, LogLevel level = LogLevel.Info) => Log(level, message, "system");
    public static void System<T>(T message, LogLevel level = LogLevel.Info) where T: class => Log(level, message, "system");
    public static void App(string message, LogLevel level = LogLevel.Info) => Log(level, message, "app");
    public static void App<T>(T message, LogLevel level = LogLevel.Info) where T: class => Log(level, message, "app");
    
    public static void Access(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "access");
    public static void Access<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "access");
    public static void Configuration(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "configuration");
    public static void Configuration<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "configuration");
    public static void Auth(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "auth");
    public static void Auth<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "auth");
    public static void Register(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "register");
    public static void Register<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "register");
    public static void Certificate(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "certificate");
    public static void Certificate<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "certificate");
    public static void Setup(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "setup");
    public static void Setup<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "setup");
    public static void Queue(string message, LogLevel level = LogLevel.Verbose) => Log(level, message, "queue");
    public static void Queue<T>(T message, LogLevel level = LogLevel.Verbose) where T: class => Log(level, message, "queue");
    public static void Encoder(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "encoder");
    public static void Encoder<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "encoder");
    public static void Ripper(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "ripper");
    public static void Ripper<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "ripper");
    public static void Http(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "http");
    public static void Http<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "http");
    public static void Notify(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "notify");
    public static void Notify<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "notify");
    public static void Ping(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "ping");
    public static void Ping<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "ping");
    public static void Socket(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "socket");
    public static void Socket<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "socket");
    public static void FanArt(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "fanart");
    public static void FanArt<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "fanart");
    public static void Fingerprint(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "fingerprint");
    public static void Fingerprint<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "fingerprint");
    public static void MovieDb(string message, LogLevel level = LogLevel.Info) => Log(level, message, "moviedb");
    public static void MovieDb<T>(T message, LogLevel level = LogLevel.Info) where T: class => Log(level, message, "moviedb");
    public static void MusicBrainz(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "musicbrainz");
    public static void MusicBrainz<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "musicbrainz");
    public static void Tvdb(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "tvdb");
    public static void Tvdb<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "tvdb");
    public static void Youtube(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "youtube");
    public static void Youtube<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "youtube");
    public static void OpenSubs(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "opensubs");
    public static void OpenSubs<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "opensubs");
    public static void QBitTorrent(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "qbittorrent");
    public static void QBitTorrent<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "qbittorrent");
    public static void Transmission(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "transmission");
    public static void Transmission<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "transmission");
    public static void RuTorrent(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "rutorrent");
    public static void RuTorrent<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "rutorrent");
    public static void SabNzbd(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "sabnzbd");
    public static void SabNzbd<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "sabnzbd");
    public static void Discord(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "discord");
    public static void Discord<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "discord");
    public static void Twitter(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "twitter");
    public static void Twitter<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "twitter");
    public static void Whatsapp(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "whatsapp");
    public static void Whatsapp<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "whatsapp");
    public static void Telegram(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "telegram");
    public static void Telegram<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "telegram");
    public static void Webhook(string message, LogLevel level = LogLevel.Debug) => Log(level, message, "webhook");
    public static void Webhook<T>(T message, LogLevel level = LogLevel.Debug) where T: class => Log(level, message, "webhook");
    
    private static string Spacer(string text, int rightPadding)
    {
        StringBuilder spacing = new StringBuilder();
        spacing.Append(text);
        for (int i = 0; i < rightPadding - text.Length; i++)
        {
            spacing.Append(' ');
        }
        return spacing.ToString();
    }
}