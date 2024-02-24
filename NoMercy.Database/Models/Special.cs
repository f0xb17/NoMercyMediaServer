using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Special : ColorPaletteTimeStamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("backdrop")] public string? Backdrop { get; set; }
        [JsonProperty("description")] public string? Description { get; set; }
        [JsonProperty("poster")] public string? Poster { get; set; }
        [JsonProperty("logo")] public string? Logo { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("titleSort")] public string? TitleSort { get; set; }
        [JsonProperty("creator")] public string? Creator { get; set; }
        [JsonProperty("overview")] public string? Overview { get; set; }

        public Special()
        {
        }
    }
}