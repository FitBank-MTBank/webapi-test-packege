using System.Text.Json;
using System.Text.Json.Serialization;

namespace Acquirer.Sample.Domain;

public static class JsonSerializerSettings
{
    public static JsonSerializerOptions AddJsonSerializerOptions(this JsonSerializerOptions jsonSerializerOptions)
    {
        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        return jsonSerializerOptions;
    }
}
