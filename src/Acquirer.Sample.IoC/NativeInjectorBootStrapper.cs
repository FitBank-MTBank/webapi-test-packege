using Acquirer.Sample.Application.Interfaces.Clients;
using Acquirer.Sample.Application.Interfaces.Services;
using Acquirer.Sample.Application.Services;
using Acquirer.Sample.Domain.Interfaces.Repositories;
using Acquirer.Sample.External.Api;
using Acquirer.Sample.Infrastructure.Repositories;
using Acquirer.Sample.IoC.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Acquirer.Sample.IoC;

public static class NativeInjectorBootStrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind / Options
        var resilienceOptions = new ResilienceOptions();
        configuration.GetSection("Resilience").Bind(resilienceOptions);

        // Services
        services
            .AddScoped<ISampleService, SampleService>();

        
        services.ConfigureExternalApiApi(configuration);

        return services;
    }

    private static IServiceCollection ConfigureExternalApiApi(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Another");

        var options = new ExternalApiOptions();
        section.Bind(options);

        services
            .AddRefitClient<IExternalApi>()
            .AddHttpMessageHandler(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<LoggingHandler>>();
                return new LoggingHandler(logger);
            })
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = options.GetUrl();
                //c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            });

        services.Configure<ExternalApiOptions>(section);

        services.AddScoped<ISampleExternalApiService, ExternalApiService>();

        return services;
    }
}

public class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Method} {RequestUri}", request.Method, request.RequestUri);

        if (request.Content != default)
        {
            logger.LogInformation("{Request}", await request.Content.ReadAsStringAsync(cancellationToken));
        }

        var response = await base.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogInformation("{ResponseContent}", responseContent);

        return response;
    }
}