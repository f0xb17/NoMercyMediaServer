// ReSharper disable InconsistentNaming

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using NoMercy.Database.Models;
using NoMercy.Helpers;
using File = NoMercy.Database.Models.File;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Database;

public class MediaContext : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={AppFiles.MediaDatabase};Pooling=True", sqliteOptionsAction =>
        {
            sqliteOptionsAction.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
        // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<string>()
            .HaveMaxLength(256);
        
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(Ulid))
            .ToList()
            .ForEach(p => p.SetElementType(typeof(string)));
        
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.Name == "CreatedAt" || p.Name == "UpdatedAt")
            .ToList()
            .ForEach(p => p.SetDefaultValueSql("CURRENT_TIMESTAMP"));
        
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .ToList()
            .ForEach(p => p.DeleteBehavior = DeleteBehavior.Cascade);

        modelBuilder.Entity<Cast>()
            .Property(t => t.RoleId)
            .IsRequired(false);
        
        modelBuilder.Entity<Crew>()
            .Property(t => t.JobId)
            .IsRequired(false);

        
        base.OnModelCreating(modelBuilder);
    }
    
    public virtual DbSet<ActivityLog> ActivityLogs { get; init; }
    public virtual DbSet<AlbumArtist> AlbumArtist { get; init; }
    public virtual DbSet<AlbumLibrary> AlbumLibrary { get; init; }
    public virtual DbSet<AlbumMusicGenre> AlbumMusicGenre { get; init; }
    public virtual DbSet<AlbumTrack> AlbumTrack { get; init; }
    public virtual DbSet<AlbumUser> AlbumUser { get; init; }
    public virtual DbSet<Album> Albums { get; init; }
    public virtual DbSet<AlternativeTitle> AlternativeTitles { get; init; }
    public virtual DbSet<ArtistLibrary> ArtistLibrary { get; init; }
    public virtual DbSet<ArtistMusicGenre> ArtistMusicGenre { get; init; }
    public virtual DbSet<ArtistTrack> ArtistTrack { get; init; }
    public virtual DbSet<ArtistUser> ArtistUser { get; init; }
    public virtual DbSet<Artist> Artists { get; init; }
    public virtual DbSet<Cast> Casts { get; init; }
    public virtual DbSet<CertificationMovie> CertificationMovie { get; init; }
    public virtual DbSet<CertificationTv> CertificationTv { get; init; }
    public virtual DbSet<Certification> Certifications { get; init; }
    public virtual DbSet<CollectionLibrary> CollectionLibrary { get; init; }
    public virtual DbSet<CollectionMovie> CollectionMovie { get; init; }
    public virtual DbSet<Collection> Collections { get; init; }
    public virtual DbSet<Configuration> Configuration { get; init; }
    public virtual DbSet<Country> Countries { get; init; }
    public virtual DbSet<Creator> Creators { get; init; }
    public virtual DbSet<Crew> Crews { get; init; }
    public virtual DbSet<Device> Devices { get; init; }
    public virtual DbSet<EncoderProfileFolder> EncoderProfileFolder { get; init; }
    public virtual DbSet<EncoderProfile> EncoderProfiles { get; init; }
    public virtual DbSet<Episode> Episodes { get; init; }
    public virtual DbSet<FileLibrary> FileLibrary { get; init; }
    public virtual DbSet<FileMovie> FileMovie { get; init; }
    public virtual DbSet<File> Files { get; init; }
    public virtual DbSet<FolderLibrary> FolderLibrary { get; init; }
    public virtual DbSet<Folder> Folders { get; init; }
    public virtual DbSet<GenreMovie> GenreMovie { get; init; }
    public virtual DbSet<GenreTv> GenreTv { get; init; }
    public virtual DbSet<Genre> Genres { get; init; }
    public virtual DbSet<GuestStar> GuestStars { get; init; }
    public virtual DbSet<Image> Images { get; init; }
    
    public virtual DbSet<Job> Jobs { get; init; }
    public virtual DbSet<KeywordMovie> KeywordMovie { get; init; }
    public virtual DbSet<KeywordTv> KeywordTv { get; init; }
    public virtual DbSet<Keyword> Keywords { get; init; }
    public virtual DbSet<LanguageLibrary> LanguageLibrary { get; init; }
    public virtual DbSet<Language> Languages { get; init; }
    public virtual DbSet<Library> Libraries { get; init; }
    public virtual DbSet<LibraryMovie> LibraryMovie { get; init; }
    public virtual DbSet<LibraryTv> LibraryTv { get; init; }
    public virtual DbSet<LibraryUser> LibraryUser { get; init; }
    public virtual DbSet<CollectionUser> CollectionUser { get; init; }
    public virtual DbSet<MovieUser> MovieUser { get; init; }
    public virtual DbSet<TvUser> TvUser { get; init; }
    public virtual DbSet<MediaAttachment> MediaAttachments { get; init; }
    public virtual DbSet<Media> Medias { get; init; }
    public virtual DbSet<MediaStream> MediaStreams { get; init; }
    public virtual DbSet<Message> Messages { get; init; }
    public virtual DbSet<Metadata> Metadata { get; init; }
    public virtual DbSet<Movie> Movies { get; init; }
    public virtual DbSet<MusicPlay> MusicPlays { get; init; }
    public virtual DbSet<MusicGenreTrack> MusicGenreTrack { get; init; }
    public virtual DbSet<MusicGenre> MusicGenres { get; init; }
    public virtual DbSet<NotificationUser> NotificationUser { get; init; }
    public virtual DbSet<Notification> Notifications { get; init; }
    public virtual DbSet<Person> People { get; init; }
    public virtual DbSet<PlaylistTrack> PlaylistTrack { get; init; }
    public virtual DbSet<Playlist> Playlists { get; init; }
    public virtual DbSet<PriorityProvider> PriorityProvider { get; init; }
    public virtual DbSet<Provider> Providers { get; init; }
    public virtual DbSet<Recommendation> Recommendations { get; init; }
    public virtual DbSet<Role> Roles { get; init; }
    public virtual DbSet<RunningTask> RunningTasks { get; init; }
    public virtual DbSet<Season> Seasons { get; init; }
    public virtual DbSet<Similar> Similar { get; init; }
    public virtual DbSet<SpecialItem> SpecialItems { get; init; }
    public virtual DbSet<Special> Specials { get; init; }
    public virtual DbSet<TrackUser> TrackUser { get; init; }
    public virtual DbSet<Track> Tracks { get; init; }
    public virtual DbSet<Translation> Translations { get; init; }
    public virtual DbSet<Tv> Tvs { get; init; }
    public virtual DbSet<UserData> UserData { get; init; }
    public virtual DbSet<User> Users { get; init; }
    public virtual DbSet<VideoFile> VideoFiles { get; init; }
}

public class Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("CURRENT_TIMESTAMP")]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
        
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("CURRENT_TIMESTAMP")]
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class ColorPalettes
{
    [Column("ColorPalette")]
    [StringLength(1024)]
    [JsonProperty("color_palette")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string _colorPalette { get; set; } = string.Empty;
    
    [NotMapped]    
    public IColorPalettes? ColorPalette
    {
        get => (_colorPalette != string.Empty 
            ? JsonConvert.DeserializeObject<IColorPalettes>(_colorPalette) 
            : null);
        set => _colorPalette = JsonConvert.SerializeObject(value);
    }
}

public class ColorPaletteTimeStamps: Timestamps
{
    [Column("ColorPalette")]
    [StringLength(1024)]
    [System.Text.Json.Serialization.JsonIgnore]
    public string _colorPalette { get; set; } = string.Empty;
    
    [NotMapped]
    public IColorPalettes? ColorPalette
    {
        get => (_colorPalette != string.Empty 
            ? JsonConvert.DeserializeObject<IColorPalettes>(_colorPalette) 
            : null);
        set => _colorPalette = JsonConvert.SerializeObject(value);
    }
}

public class IColorPalettes
{
    [JsonProperty("poster")]
    public IPalette? Poster { get; set; }
    [JsonProperty("backdrop")]
    public IPalette? Backdrop { get; set; }
    [JsonProperty("still")]
    public IPalette? Still { get; set; }
    [JsonProperty("profile")]
    public IPalette? Profile { get; set; }
    [JsonProperty("image")]
    public IPalette? Image { get; set; }
}

public class IPalette
{
    public string Primary { get; set; }
    public string LightVibrant { get; set; }
    public string DarkVibrant { get; set; }
    public string LightMuted { get; set; }
    public string DarkMuted { get; set; }
}

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints defaultHints = new(size: 26);

    public UlidToStringConverter() : this(null)
    {
    }

    private UlidToStringConverter(ConverterMappingHints? mappingHints = null)
        : base(
            convertToProviderExpression: x => x.ToString(),
            convertFromProviderExpression: x => Ulid.Parse(x),
            mappingHints: defaultHints.With(mappingHints))
    {
    }
}
