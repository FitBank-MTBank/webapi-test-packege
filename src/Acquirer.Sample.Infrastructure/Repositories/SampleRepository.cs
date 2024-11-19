using Acquirer.Sample.Domain.Interfaces.Repositories;

namespace Acquirer.Sample.Infrastructure.Repositories;

public class SampleRepository : BaseRepository<Domain.Entities.Sample, Guid>, ISampleRepository
{
    public SampleRepository(SampleDbContext context) : base(context) { }
}