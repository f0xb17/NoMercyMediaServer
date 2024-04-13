namespace NoMercy.Providers.MusixMatch.Models;

public class TrackSearchParameters
{
    public static Dictionary<SortStrategy, KeyValuePair<string, string>> StrategyDecryptions = new()
    {
        [SortStrategy.TrackRatingAsc] = new KeyValuePair<string, string>("s_track_rating", "asc"),
        [SortStrategy.TrackRatingDesc] = new KeyValuePair<string, string>("s_track_rating", "desc"),
        [SortStrategy.ArtistRatingAsc] = new KeyValuePair<string, string>("s_artist_rating", "asc"),
        [SortStrategy.ArtistRatingDesc] = new KeyValuePair<string, string>("s_artist_rating", "desc"),
        [SortStrategy.ReleaseDateAsc] = new KeyValuePair<string, string>("s_track_release_date", "asc"),
        [SortStrategy.ReleaseDateDesc] = new KeyValuePair<string, string>("s_track_release_date", "desc")
    };

    public string? Query { get; set; } = "";
    public string? LyricsQuery { get; set; } = "";
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string[]? Artists { get; set; }
    public string Album { get; set; } = "";
    public string? Duration { get; set; } = "";
    public string? Language { get; set; } = "";
    public bool? HasLyrics { get; set; } = true;
    public bool? HasSubtitles { get; set; }
    public bool? HasRichSync { get; set; }
    public SortStrategy? Sort { get; set; } = SortStrategy.TrackRatingDesc;
    

    public enum SortStrategy
    {
        TrackRatingAsc,
        TrackRatingDesc,
        ArtistRatingAsc,
        ArtistRatingDesc,
        ReleaseDateAsc,
        ReleaseDateDesc,
    }
}