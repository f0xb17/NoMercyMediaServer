namespace NoMercy.Server.Helpers;
using Microsoft.EntityFrameworkCore;

public class MediaDbContext() : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={AppFiles.MediaDatabase}");
    }
}
public class QueueDbContext() : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={AppFiles.QueueDatabase}");
    }
}