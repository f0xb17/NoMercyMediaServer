using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Configuration : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")] public required string Id { get; set; }

        [JsonProperty("key")] public required string Key { get; set; }

        [JsonProperty("value")] public string? Value { get; set; }

        [JsonProperty("modified_by")] public string? ModifiedBy { get; set; }
    }
}