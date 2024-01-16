// ReSharper disable InconsistentNaming

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using File = NoMercy.Database.Models.File;

namespace NoMercy.Database;

public class Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("CURRENT_TIMESTAMP")]
    public DateTime? CreatedAt { get; set; }
        
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("CURRENT_TIMESTAMP")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}

public class ColorPalettes
{
    [Column("ColorPalette")]
    [StringLength(1024)]
    public string _colorPalette { get; set; } = string.Empty;
    
    [NotMapped]
    public Dictionary<string, string>? ColorPalette
    {
        get => JsonConvert.DeserializeObject<Dictionary<string, string>>(_colorPalette);
        set => _colorPalette = JsonConvert.SerializeObject(value);
    }
}

public class ColorPaletteTimeStamps: Timestamps
{
    [Column("ColorPalette")]
    [StringLength(1024)]
    public string _colorPalette { get; set; } = string.Empty;
    
    [NotMapped]
    public dynamic? ColorPalette
    {
        get => _colorPalette != string.Empty ? JsonConvert.DeserializeObject<dynamic>(_colorPalette) : null;
        set => _colorPalette = JsonConvert.SerializeObject(value);
    }
}

public class MediaContext : DbContext
{
    public MediaContext(DbContextOptions<MediaContext> options) : base(options)
    {
    }

    public MediaContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={AppFiles.MediaDatabase}");
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<string>()
            .HaveMaxLength(256);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.Name == "CreatedAt" || p.Name == "UpdatedAt")
            .ToList()
            .ForEach(p => p.SetDefaultValueSql("CURRENT_TIMESTAMP"));
        
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .ToList()
            .ForEach(p => p.DeleteBehavior = DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
    public virtual DbSet<Album_Artist> Album_Artist { get; set; }
    public virtual DbSet<Album_Library> Album_Library { get; set; }
    public virtual DbSet<Album_MusicGenre> Album_MusicGenre { get; set; }
    public virtual DbSet<Album_Track> Album_Track { get; set; }
    public virtual DbSet<Album_User> Album_User { get; set; }
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<AlternativeTitle> AlternativeTitles { get; set; }
    public virtual DbSet<Artist_Library> Artist_Library { get; set; }
    public virtual DbSet<Artist_MusicGenre> Artist_MusicGenre { get; set; }
    public virtual DbSet<Artist_Track> Artist_Track { get; set; }
    public virtual DbSet<Artist_User> Artist_User { get; set; }
    public virtual DbSet<Artist> Artists { get; set; }
    public virtual DbSet<Cast> Casts { get; set; }
    public virtual DbSet<Certification_Movie> Certification_Movie { get; set; }
    public virtual DbSet<Certification_Tv> Certification_Tv { get; set; }
    public virtual DbSet<Certification> Certifications { get; set; }
    public virtual DbSet<Collection_Library> Collection_Library { get; set; }
    public virtual DbSet<Collection_Movie> Collection_Movie { get; set; }
    public virtual DbSet<Collection> Collections { get; set; }
    public virtual DbSet<Configuration> Configuration { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Creator> Creators { get; set; }
    public virtual DbSet<Crew> Crews { get; set; }
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<EncoderProfile_Library> EncoderProfile_Library { get; set; }
    public virtual DbSet<EncoderProfile> EncoderProfiles { get; set; }
    public virtual DbSet<Episode> Episodes { get; set; }
    public virtual DbSet<File_Library> File_Library { get; set; }
    public virtual DbSet<File_Movie> File_Movie { get; set; }
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Folder_Library> Folder_Library { get; set; }
    public virtual DbSet<Folder> Folders { get; set; }
    public virtual DbSet<Genre_Movie> Genre_Movie { get; set; }
    public virtual DbSet<Genre_Tv> Genre_Tv { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<GuestStar> GuestStars { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    
    public virtual DbSet<Keyword_Movie> Keyword_Movie { get; set; }
    public virtual DbSet<Keyword_Tv> Keyword_Tv { get; set; }
    public virtual DbSet<Keyword> Keywords { get; set; }
    public virtual DbSet<Language_Library> Language_Library { get; set; }
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }
    public virtual DbSet<Library_Movie> Library_Movie { get; set; }
    public virtual DbSet<Library_Tv> Library_Tv { get; set; }
    public virtual DbSet<Library_User> Library_User { get; set; }
    public virtual DbSet<MediaAttachment> MediaAttachments { get; set; }
    public virtual DbSet<Media> Medias { get; set; }
    public virtual DbSet<MediaStream> MediaStreams { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<Metadata> Metadata { get; set; }
    public virtual DbSet<Movie> Movies { get; set; }
    public virtual DbSet<Music_Play> Music_Plays { get; set; }
    public virtual DbSet<MusicGenre_Track> MusicGenre_Track { get; set; }
    public virtual DbSet<MusicGenre> MusicGenres { get; set; }
    public virtual DbSet<Notification_User> Notification_User { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<Playlist_Track> Playlist_Track { get; set; }
    public virtual DbSet<Playlist> Playlists { get; set; }
    public virtual DbSet<Priority_Provider> Priority_Provider { get; set; }
    public virtual DbSet<Provider> Providers { get; set; }
    public virtual DbSet<Recommendation> Recommendations { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RunningTask> RunningTasks { get; set; }
    public virtual DbSet<Season> Seasons { get; set; }
    public virtual DbSet<Similar> Similar { get; set; }
    public virtual DbSet<SpecialItem> SpecialItems { get; set; }
    public virtual DbSet<Special> Specials { get; set; }
    public virtual DbSet<Track_User> Track_User { get; set; }
    public virtual DbSet<Track> Tracks { get; set; }
    public virtual DbSet<Translation> Translations { get; set; }
    public virtual DbSet<Tv> Tvs { get; set; }
    public virtual DbSet<UserData> UserData { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<VideoFile> VideoFiles { get; set; }
    
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<FailedJob> FailedJobs { get; set; }
}