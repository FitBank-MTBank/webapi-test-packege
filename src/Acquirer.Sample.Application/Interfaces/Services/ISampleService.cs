using Acquirer.Sample.Application.Models.Sample;

namespace Acquirer.Sample.Application.Interfaces.Services;

public interface ISampleService : IBaseService<Domain.Entities.Sample, SampleCreateModel, SampleUpdateModel, SampleResultModel, Guid>
{    
}