using Acquirer.Sample.Application.Interfaces.Clients;
using Acquirer.Sample.Application.Models.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Acquirer.Sample.External.Api;

public class ExternalApiService(IExternalApi api, IOptions<ExternalApiOptions> options, ILogger<ExternalApiService> logger) : ISampleExternalApiService
{
    private readonly ExternalApiOptions _options = options.Value;

    public async Task<SampleExternalApiResponseModel> GenericGetFromAnotherApiMethod(Guid id)
    {
        try
        {
            var response = await api.GenericGetMethod(id);

            var parseSuccess = bool.TryParse(response.Success, out var success);

            if (!success || !parseSuccess)
            {
                logger.LogError($"GenericApi Error: {response.Message}");
                return null;
            }

            if (response.Message is null or "")
            {
                return null;
            }

            return JsonConvert.DeserializeObject<SampleExternalApiResponseModel>(response.Message);
        }
        catch (Exception e)
        {
            logger.LogError($"Exception: {e}");
            return null;
        }
    }
}