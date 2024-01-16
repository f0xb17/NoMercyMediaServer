using Microsoft.EntityFrameworkCore;

namespace NoMercy.Database.Models
{
    [PrimaryKey(nameof(EncoderProfileId), nameof(LibraryId))]
    public class EncoderProfile_Library
    {
        public required string EncoderProfileId { get; set; }
        public required string LibraryId { get; set; }

        public virtual EncoderProfile EncoderProfile { get; } = null!;
        public virtual Library Library { get; set; } = null!;
    }
}