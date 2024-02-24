using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class PersonTranslations
{
    [JsonProperty("translations")] public PersonTranslation[] Translations { get; set; } = [];
}

public class PersonTranslation
{
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }

    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("data")] public Data Data { get; set; } = new();

    [JsonProperty("english_name")] public string EnglishName { get; set; }
}

public class Data
{
    [JsonProperty("biography")] public string? Overview { get; set; }
}