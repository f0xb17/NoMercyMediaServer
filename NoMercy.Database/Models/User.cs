using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(Id))]
    public class User: Timestamps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool Manage { get; set; }
        public bool Owner { get; set; }
        public string Name { get; set; }
        public bool Allowed { get; set; }
        public bool AudioTranscoding { get; set; }
        public bool VideoTranscoding { get; set; }
        public bool NoTranscoding { get; set; }

        public User()
        { }
        
        public User(Guid id, string email, bool manage, bool owner, string name, bool allowed, bool audioTranscoding, bool videoTranscoding, bool noTranscoding)
        {
            Id = id;
            Email = email;
            Manage = manage;
            Owner = owner;
            Name = name;
            Allowed = allowed;
            AudioTranscoding = audioTranscoding;
            VideoTranscoding = videoTranscoding;
            NoTranscoding = noTranscoding;
        }

    }
}