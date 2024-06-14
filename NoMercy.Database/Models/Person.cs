#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.People;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Person : ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("also_known_as")] public string? AlsoKnownAs { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }
    [JsonProperty("birthday")] public DateTime? BirthDay { get; set; }
    [JsonProperty("death_day")] public DateTime? DeathDay { get; set; }

    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("profile")] public string? Profile { get; set; }
    [JsonProperty("title_sort")] public string TitleSort { get; set; } = string.Empty;

    [JsonProperty("casts")] public ICollection<Cast> Casts { get; set; }
    [JsonProperty("crews")] public ICollection<Crew> Crews { get; set; }
    [JsonProperty("images")] public ICollection<Image> Images { get; set; }
    [JsonProperty("translations")] public ICollection<Translation> Translations { get; set; }

    [Column("Gender")]
    [JsonProperty("gender")]
    [System.Text.Json.Serialization.JsonIgnore]

    public TmdbGender TmdbGender { get; set; }

    [NotMapped]
    [JsonProperty("Gender")]
    public string Gender
    {
        get => TmdbGender.ToString();
        set => TmdbGender = Enum.Parse<TmdbGender>(value);
    }

    [Column("ExternalIds")]
    [JsonProperty("external_ids")]
    [System.Text.Json.Serialization.JsonIgnore]

    public string? _externalIds { get; set; }

    [NotMapped]
    public TmdbPersonExternalIds? ExternalIds
    {
        get => _externalIds is null ? null : JsonConvert.DeserializeObject<TmdbPersonExternalIds>(_externalIds);
        set => _externalIds = JsonConvert.SerializeObject(value);
    }

    public Person()
    {
    }

    public Person(TmdbPersonAppends? person)
    {
        Id = person.Id;
        Adult = person.Adult;
        AlsoKnownAs = person.AlsoKnownAs.Length > 0 ? JsonConvert.SerializeObject(person.AlsoKnownAs) : null;
        Biography = person.Biography;
        BirthDay = person.BirthDay;
        DeathDay = person.DeathDay;
        TmdbGender = person.TmdbGender;
        _externalIds = JsonConvert.SerializeObject(person.ExternalIds);
        Homepage = person.Homepage?.ToString();
        ImdbId = person.ImdbId;
        KnownForDepartment = person.KnownForDepartment;
        Name = person.Name;
        PlaceOfBirth = person.PlaceOfBirth;
        Popularity = person.Popularity;
        Profile = person.ProfilePath;
        TitleSort = person.Name;
    }
}