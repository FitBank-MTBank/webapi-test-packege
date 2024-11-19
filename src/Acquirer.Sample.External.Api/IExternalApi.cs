using Acquirer.Sample.Application.Models;
using Refit;

namespace Acquirer.Sample.External.Api;

public interface IExternalApi
{
    [Post("/api/v1/another")]
    Task<ExternalApiResponse> GenericGetMethod(Guid id);
}