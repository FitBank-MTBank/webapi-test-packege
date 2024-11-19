using Acquirer.Sample.Domain.Interfaces.Repositories;
using Acquirer.Sample.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acquirer.Sample.Infrastructure.Extensions;

public static class InfrastructureSettingsExtension
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<SampleDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("SampleDbContext"))
             .UseSnakeCaseNamingConvention()
             .EnableSensitiveDataLogging(bool.Parse(configuration["DbContextOptions:SensitiveDataLoggingEnabled"] ?? "false"))
             .EnableDetailedErrors(bool.Parse(configuration["DbContextOptions:DetailedErrorsEnabled"] ?? "false")));

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ISampleRepository, SampleRepository>();

    }
}