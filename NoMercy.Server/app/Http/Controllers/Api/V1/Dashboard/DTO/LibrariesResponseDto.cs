#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Dashboard.DTO;

public class LibrariesResponseDto
{
    [JsonProperty("data")] public IEnumerable<LibrariesResponseItemDto>? Data { get; set; }
    
    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<Library?>> GetLibraries =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Libraries.AsNoTracking()
                .Where(library => library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null
                )
                .Include(library => library.FolderLibraries)
                    .ThenInclude(folderLibrary => folderLibrary.Folder)
                        .ThenInclude(folder => folder.EncoderProfileFolder)
                            .ThenInclude(library => library.EncoderProfile)
                
                .Include(library => library.LanguageLibraries)
                .ThenInclude(languageLibrary => languageLibrary.Language));
}

public class LibrariesResponseItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("autoRefreshInterval")] public long AutoRefreshInterval { get; set; }
    [JsonProperty("chapterImages")] public long ChapterImages { get; set; }
    [JsonProperty("image")] public string? Image { get; set; }
    [JsonProperty("perfectSubtitleMatch")] public bool PerfectSubtitleMatch { get; set; }
    [JsonProperty("realtime")] public bool Realtime { get; set; }
    [JsonProperty("specialSeasonName")] public string? SpecialSeasonName { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }

    [JsonProperty("color_palette")] 
    public object? ColorPalette { get; set; }

    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
    [JsonProperty("folder_library")] public FolderLibraryDto[] FolderLibrary { get; set; }
    [JsonProperty("subtitles")] public string[] Subtitles { get; set; }
    
    public LibrariesResponseItemDto(Library library)
    {
        Id = library.Id;
        AutoRefreshInterval = library.AutoRefreshInterval;
        Image = library.Image;
        PerfectSubtitleMatch = library.PerfectSubtitleMatch;
        Realtime = library.Realtime;
        SpecialSeasonName = library.SpecialSeasonName;
        Title = library.Title;
        Type = library.Type;
        Order = library.Order;
        CreatedAt = library.CreatedAt;
        UpdatedAt = library.UpdatedAt;
        
        Subtitles = library.LanguageLibraries
            .Select(languageLibrary => languageLibrary.Language.Iso6391)
            .ToArray();

        FolderLibrary = library.FolderLibraries
            .Select(folderLibrary => new FolderLibraryDto
            {
                FolderId = folderLibrary.FolderId,
                LibraryId = folderLibrary.LibraryId,
                Folder = new FolderDto
                {
                    Id = folderLibrary.Folder.Id,
                    Path = folderLibrary.Folder.Path,
                    EncoderProfiles = folderLibrary.Folder.EncoderProfileFolder
                        .Select(encoderProfileFolder => encoderProfileFolder.EncoderProfile.Id)
                        .ToArray()
                }
            })
            .ToArray();

    }

}

public class FolderLibraryDto
{
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder")] public FolderDto Folder { get; set; }
}

public class FolderDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("encoder_profiles")] public Ulid[] EncoderProfiles { get; set; }
}