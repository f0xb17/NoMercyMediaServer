using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class LyricsUser
{
    [JsonProperty("user")] public User User;
}

public class User
{
    [JsonProperty("uaid")] public string Uaid;
    [JsonProperty("is_mine")] public int IsMine;
    [JsonProperty("user_name")] public string UserName;
    [JsonProperty("user_profile_photo")] public string UserProfilePhoto;
    [JsonProperty("has_private_profile")] public int HasPrivateProfile;
    [JsonProperty("score")] public int Score;
    [JsonProperty("position")] public int Position;
    [JsonProperty("weekly_score")] public int WeeklyScore;
    [JsonProperty("level")] public string Level;
    [JsonProperty("key")] public string Key;
    [JsonProperty("rank_level")] public int RankLevel;
    [JsonProperty("points_to_next_level")] public int PointsToNextLevel;
    [JsonProperty("ratio_to_next_level")] public double RatioToNextLevel;
    [JsonProperty("rank_name")] public string RankName;
    [JsonProperty("next_rank_name")] public string NextRankName;
    [JsonProperty("ratio_to_next_rank")] public double RatioToNextRank;
    [JsonProperty("rank_color")] public string RankColor;
    [JsonProperty("rank_colors")] public RankColors RankColors;
    [JsonProperty("rank_image_url")] public string RankImageUrl;
    [JsonProperty("next_rank_color")] public string NextRankColor;
    [JsonProperty("next_rank_colors")] public RankColors NextRankColors;
    [JsonProperty("next_rank_image_url")] public string NextRankImageUrl;
    [JsonProperty("counters")] public Counters Counters;
    [JsonProperty("academy_completed")] public bool AcademyCompleted;
    [JsonProperty("moderator")] public bool Moderator;
}

public class RankColors
{
    [JsonProperty("rank_color_10")] public string RankColor10;
    [JsonProperty("rank_color_50")] public string RankColor50;
    [JsonProperty("rank_color_100")] public string RankColor100;
    [JsonProperty("rank_color_200")] public string RankColor200;
}
