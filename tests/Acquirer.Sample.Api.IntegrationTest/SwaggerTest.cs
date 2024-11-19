using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Acquirer.Sample.Api.IntegrationTest;

public class SwaggerTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task Get_Swagger_Should_Success()
    {
        var response = await _httpClient.GetAsync("/swagger/index.html");
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEmpty(await response.Content.ReadAsStringAsync());
    }
}