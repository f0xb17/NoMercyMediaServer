using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Path), IsUnique = true)]
    public class Folder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id")]
        public Ulid Id { get; set; }

        [JsonProperty("path")] public string Path { get; set; }

        
        [JsonProperty("encoder_profile_folder")]
        public virtual ICollection<EncoderProfileFolder> EncoderProfileFolder { get; set; }
    }
}