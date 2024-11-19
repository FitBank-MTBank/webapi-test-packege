using Acquirer.Sample.Domain.Entities;
using Acquirer.Sample.Infrastructure.Persistence.Inserts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Acquirer.Sample.Infrastructure.Persistence;

public class SampleDbContext(
    DbContextOptions<SampleDbContext> options)
    : DbContext(options)
{
    public DbSet<Domain.Entities.Sample> Orders => Set<Domain.Entities.Sample>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<BaseEntity>();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        PopulateContext(modelBuilder);

        ApplyRestrictedDelete(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void PopulateContext(ModelBuilder modelBuilder)
    {
        modelBuilder.PopulateSamples();
    }

    private static void ApplyRestrictedDelete(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private void AddParameterBaseEntities()
    {
        var baseEntities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in baseEntities)
        {
            var now = DateTime.Now;

            var baseEntity = (entity.Entity as BaseEntity);

            if (entity.State == EntityState.Added)
            {
                if(baseEntity.Created == default)
                {
                    baseEntity.Created = now;
                }
                
                baseEntity.Excluded = false;
            }

            baseEntity.Updated = now;
        }
    }

    public override int SaveChanges()
    {
        AddParameterBaseEntities();
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddParameterBaseEntities();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}