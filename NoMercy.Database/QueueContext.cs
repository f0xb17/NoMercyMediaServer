// ReSharper disable InconsistentNaming

using Microsoft.EntityFrameworkCore;
using NoMercy.Database.Models;
using NoMercy.Helpers;

namespace NoMercy.Database;

public class QueueContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={AppFiles.QueueDatabase};Pooling=True", sqliteOptionsAction =>
        {
            sqliteOptionsAction.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
        });
        
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<string>()
            .HaveMaxLength(256);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.Name is "CreatedAt" or "UpdatedAt")
            .ToList()
            .ForEach(p => p.SetDefaultValueSql("CURRENT_TIMESTAMP"));
        
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .ToList()
            .ForEach(p => p.DeleteBehavior = DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public virtual DbSet<QueueJob> QueueJobs { get; set; }
    public virtual DbSet<FailedJob> FailedJobs { get; set; }
}
