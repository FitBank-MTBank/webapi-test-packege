using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Acquirer.Sample.Api.IntegrationTest;

public class HealthTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task Get_Health_Should_Success()
    {
        var response = await _httpClient.GetAsync("/health");
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Get_Health_Status_Should_Success()
    {
        var response = await _httpClient.GetAsync("/health/status");
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
    }
}