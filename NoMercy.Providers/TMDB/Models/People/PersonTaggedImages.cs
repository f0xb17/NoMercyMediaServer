using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.Shared;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonTaggedImages : PaginatedResponse<TaggedImage>
{
    [JsonProperty("id")] public string Id { get; set; }
}

public class TaggedImage : Profile
{
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;

    [JsonProperty("image_type")] public string ImageType { get; set; } = string.Empty;

    [JsonProperty("media")] public Media Media { get; set; } = new();

    [JsonProperty("media_type")] public string MediaType { get; set; } = string.Empty;
}

public class Media
{
    [JsonProperty("_id")] public string Id { get; set; } = string.Empty;

    [JsonProperty("id")] public int MediaId { get; set; }

    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
}