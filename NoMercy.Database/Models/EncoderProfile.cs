using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class EncoderProfile : Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        [Key] 
        [JsonProperty("name")] public required string Name { get; set; }

        [JsonProperty("container")] public string? Container { get; set; }

        [JsonProperty("param")] public string? Param { get; set; }
        
        [JsonProperty("encoder_profile_folder")]
        public virtual ICollection<EncoderProfileFolder> EncoderProfileFolder { get; set; }
    }
}