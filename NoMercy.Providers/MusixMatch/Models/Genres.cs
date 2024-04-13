using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class Genres
{
    [JsonProperty("music_genre_list")] public MusicGenreList[] MusicGenreList { get; set; }
}

public class MusicGenreList
{
    [JsonProperty("music_genre")] public MusicGenre MusicGenre { get; set; }
}