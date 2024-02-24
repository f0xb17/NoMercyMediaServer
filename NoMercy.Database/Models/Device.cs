using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class Device : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public required Ulid Id { get; set; } = Ulid.NewUlid();

        [JsonProperty("device_id")] public required string DeviceId { get; set; }

        [JsonProperty("user_id")] public required string Browser { get; set; }

        [JsonProperty("os")] public required string Os { get; set; }

        [Column("Device")]
        [JsonProperty("model")]
        public required string Model { get; set; }

        [JsonProperty("type")] public required string Type { get; set; }

        [JsonProperty("name")] public required string Name { get; set; }

        [JsonProperty("custom_name")] public string? CustomName { get; set; }

        [JsonProperty("version")] public required string Version { get; set; }

        [JsonProperty("ip")] public required string Ip { get; set; }
    }
}