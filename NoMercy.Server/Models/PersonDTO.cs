using NoMercy.Providers.TMDB.Models.People;

namespace NoMercy.Server.Models;

public class PersonDto
{
    public PersonDto(PersonAppends response)
    {
        Id = response.Id;
        Name = response.Name;
        Adult = response.Adult;
        Gender = response.Gender.ToString();
        ProfilePath = response.ProfilePath;
        PlaceOfBirth = response.PlaceOfBirth;
        Birthday = response.BirthDay;
        DeathDay = response.DeathDay;
        KnownForDepartment = response.KnownForDepartment;
        AlsoKnownAs = response.AlsoKnownAs?.ToArray();
        Biography = response.Biography;
        Popularity = response.Popularity;
        ImdbId = response.ImdbId;
        Homepage = response.Homepage;
        Images = response.Images;
        ExternalIds = response.ExternalIds;
        Translations = response.Translations;
        MovieCredits = response.MovieCredits;
        TvCredits = response.TvCredits;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Adult { get; set; }
    public string Gender { get; set; }
    public string? ProfilePath { get; set; }
    public string? PlaceOfBirth { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? DeathDay { get; set; }

    public string? KnownForDepartment { get; set; }
    public string[]? AlsoKnownAs { get; set; }
    public string? Biography { get; set; }
    public float? Popularity { get; set; }
    public string? ImdbId { get; set; }
    public Uri? Homepage { get; set; }

    public PersonImages Images { get; set; }
    public PersonExternalIds ExternalIds { get; set; }
    public PersonTranslations Translations { get; set; }
    public PersonMovieCredits MovieCredits { get; set; }
    public PersonTvCredits TvCredits { get; set; }

}