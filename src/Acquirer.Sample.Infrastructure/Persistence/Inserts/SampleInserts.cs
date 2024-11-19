namespace Acquirer.Sample.Infrastructure.Persistence.Inserts;

public static class SampleInserts
{
    public static void PopulateSamples(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Sample>().HasData(
            new Domain.Entities.Sample
            {
                SampleId = Guid.NewGuid(),
                Description = "Teste",
                SampleType = Domain.Enums.SampleTypeEnum.OrderOne,
                DateAt = new DateTime(2024, 4, 16, 0, 0, 0, DateTimeKind.Local)
            }
        );
    }
}