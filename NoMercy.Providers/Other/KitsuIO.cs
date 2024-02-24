using Newtonsoft.Json;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Providers.Other;

public static class KitsuIO
{
    public static async Task<bool> IsAnime(string title, int year)
    {
        bool isAnime = false;
        
        var client = new HttpClient();
        var response = await client.GetAsync($"https://kitsu.io/api/edge/anime?filter[text]={title}&filter[year]={year}");
        var content = await response.Content.ReadAsStringAsync();
        var anime = JsonConvert.DeserializeObject<KitsuAnime>(content);
        
        foreach (var data in anime?.Data ?? [])
        {
            if (data.Attributes.Titles.En?.ToLower() == title.ToLower())
            {
                isAnime = true;
            }
            else if (data.Attributes.Titles.EnJp?.ToLower() == title.ToLower())
            {
                isAnime = true;
            }
            else if (data.Attributes.Titles.JaJp?.ToLower() == title.ToLower())
            {
                isAnime = true;
            }
        }
        
        return isAnime;
    }
}

public class KitsuAnime
{
    [JsonProperty("data")] public Datum[] Data { get; set; }

    [JsonProperty("meta")] public KitsuIoMeta Meta { get; set; }

    [JsonProperty("links")] public KitsuIoLinks Links { get; set; }
}

public class Datum
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("links")] public DatumLinks Links { get; set; }

    [JsonProperty("attributes")] public Attributes Attributes { get; set; }

    [JsonProperty("relationships")] public Dictionary<string, Relationship> Relationships { get; set; }
}

public class Attributes
{
    [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")] public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("slug")] public string Slug { get; set; }

    [JsonProperty("synopsis")] public string Synopsis { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("coverImageTopOffset")] public long CoverImageTopOffset { get; set; }

    [JsonProperty("titles")] public Titles Titles { get; set; }

    [JsonProperty("canonicalTitle")] public string CanonicalTitle { get; set; }

    [JsonProperty("abbreviatedTitles")] public string[] AbbreviatedTitles { get; set; }

    [JsonProperty("averageRating")] public string AverageRating { get; set; }

    [JsonProperty("ratingFrequencies")] public Dictionary<string, long> RatingFrequencies { get; set; }

    [JsonProperty("userCount")] public long UserCount { get; set; }

    [JsonProperty("favoritesCount")] public long FavoritesCount { get; set; }

    [JsonProperty("startDate")] public DateTimeOffset StartDate { get; set; }

    [JsonProperty("endDate")] public DateTimeOffset EndDate { get; set; }

    [JsonProperty("nextRelease")] public object NextRelease { get; set; }

    [JsonProperty("popularityRank")] public long PopularityRank { get; set; }

    [JsonProperty("ratingRank")] public long RatingRank { get; set; }

    [JsonProperty("ageRating")] public string AgeRating { get; set; }

    [JsonProperty("ageRatingGuide")] public string AgeRatingGuide { get; set; }

    [JsonProperty("subtype")] public string Subtype { get; set; }

    [JsonProperty("status")] public string Status { get; set; }

    [JsonProperty("tba")] public string Tba { get; set; }

    [JsonProperty("posterImage")] public PosterImage PosterImage { get; set; }

    [JsonProperty("coverImage")] public CoverImage CoverImage { get; set; }

    [JsonProperty("episodeCount")] public long EpisodeCount { get; set; }

    [JsonProperty("episodeLength")] public long EpisodeLength { get; set; }

    [JsonProperty("totalLength")] public long TotalLength { get; set; }

    [JsonProperty("youtubeVideoId")] public string YoutubeVideoId { get; set; }

    [JsonProperty("showType")] public string ShowType { get; set; }

    [JsonProperty("nsfw")] public bool Nsfw { get; set; }
}

public class CoverImage
{
    [JsonProperty("tiny")] public Uri Tiny { get; set; }

    [JsonProperty("large")] public Uri Large { get; set; }

    [JsonProperty("small")] public Uri Small { get; set; }

    [JsonProperty("original")] public Uri Original { get; set; }

    [JsonProperty("meta")] public CoverImageMeta Meta { get; set; }
}

public class CoverImageMeta
{
    [JsonProperty("dimensions")] public Dimensions Dimensions { get; set; }
}

public class Dimensions
{
    [JsonProperty("tiny")] public Large Tiny { get; set; }

    [JsonProperty("large")] public Large Large { get; set; }

    [JsonProperty("small")] public Large Small { get; set; }

    [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
    public Large Medium { get; set; }
}

public class Large
{
    [JsonProperty("width")] public long Width { get; set; }

    [JsonProperty("height")] public long Height { get; set; }
}

public class PosterImage
{
    [JsonProperty("tiny")] public Uri Tiny { get; set; }

    [JsonProperty("large")] public Uri Large { get; set; }

    [JsonProperty("small")] public Uri Small { get; set; }

    [JsonProperty("medium")] public Uri Medium { get; set; }

    [JsonProperty("original")] public Uri Original { get; set; }

    [JsonProperty("meta")] public CoverImageMeta Meta { get; set; }
}

public class Titles
{
    [JsonProperty("en")] public string En { get; set; }

    [JsonProperty("en_jp")] public string EnJp { get; set; }

    [JsonProperty("ja_jp")] public string JaJp { get; set; }
}

public class DatumLinks
{
    [JsonProperty("self")] public Uri Self { get; set; }
}

public class Relationship
{
    [JsonProperty("links")] public RelationshipLinks Links { get; set; }
}

public class RelationshipLinks
{
    [JsonProperty("self")] public Uri Self { get; set; }

    [JsonProperty("related")] public Uri Related { get; set; }
}

public class KitsuIoLinks
{
    [JsonProperty("first")] public Uri First { get; set; }

    [JsonProperty("last")] public Uri Last { get; set; }
}

public class KitsuIoMeta
{
    [JsonProperty("count")] public long Count { get; set; }
}