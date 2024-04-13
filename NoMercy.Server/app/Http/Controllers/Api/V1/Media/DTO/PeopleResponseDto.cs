using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Server.app.Http.Controllers.Api.V1.Media.DTO;

public class PeopleResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }
    [JsonProperty("data")] public IEnumerable<PeopleResponseItemDto>? Data { get; set; }
}

public class PeopleResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("alsoKnownAs")] public string[]? AlsoKnownAs { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }
    [JsonProperty("birthday")] public DateTime? Birthday { get; set; }
    [JsonProperty("colorPalette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("deathDay")] public DateTime? DeathDay { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdbId")] public string? ImdbId { get; set; }
    [JsonProperty("knownForDepartment")] public string? KnownForDepartment { get; set; }
    [JsonProperty("mediaType")] public string MediaType { get; set; }
    [JsonProperty("Type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("placeOfBirth")] public string? PlaceOfBirth { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("profile")] public string? Profile { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    
    public PeopleResponseItemDto(Person person)
    {
        string biography = person.Translations
            .FirstOrDefault()?.Biography ?? person.Biography ?? string.Empty;

        Id = person.Id;
        Name = person.Name;
        Biography = biography;
        Adult = person.Adult;
        AlsoKnownAs = person.AlsoKnownAs is null 
            ? [] 
            : JsonConvert.DeserializeObject<string[]>(person.AlsoKnownAs);
        Birthday = person.BirthDay;
        DeathDay = person.DeathDay;
        Gender = person.Gender;
        Homepage = person.Homepage;
        ImdbId = person.ImdbId;
        KnownForDepartment = person.KnownForDepartment;
        PlaceOfBirth = person.PlaceOfBirth;
        Popularity = person.Popularity;
        Profile = person.Profile;
        Poster = person.Profile;
        ColorPalette = person.ColorPalette;
        CreatedAt = person.CreatedAt;
        UpdatedAt = person.UpdatedAt;
        ColorPalette = person.ColorPalette;
        MediaType = "person";
        Type = "person";
    }

}