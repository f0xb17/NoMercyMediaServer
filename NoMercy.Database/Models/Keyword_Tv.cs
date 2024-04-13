#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Providers.TMDB.Models.TV;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(KeywordId), nameof(TvId))]
    public class KeywordTv
    {
        [JsonProperty("keyword_id")] public int KeywordId { get; set; }
        public virtual Keyword Keyword { get; set; }

        [JsonProperty("tv_id")] public int TvId { get; set; }
        public virtual Tv Tv { get; set; }

        public KeywordTv()
        {
        }

        public KeywordTv(Providers.TMDB.Models.Shared.Keyword input, TvShowAppends show)
        {
            KeywordId = input.Id;
            TvId = show.Id;
        }
    }
}