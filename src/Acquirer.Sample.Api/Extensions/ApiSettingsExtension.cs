using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Acquirer.Sample.Api.Middlewares;
using Acquirer.Sample.Domain;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;

namespace Acquirer.Sample.Api.Extensions;

public static class ApiSettingsExtension
{
    public static IServiceCollection AddApiSettings(this IServiceCollection services)
    {
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddJsonOptions(options => { options.JsonSerializerOptions.AddJsonSerializerOptions(); });
        services.AddHealthChecks();

        return services;
    }

    public static IServiceCollection AddApiTools(this IServiceCollection services) =>
        services.AddApiVersioning(p =>
        {
            p.DefaultApiVersion = new ApiVersion(1, 0);
            p.ReportApiVersions = true;
            p.AssumeDefaultVersionWhenUnspecified = true;
        }).AddVersionedApiExplorer(p =>
        {
            p.GroupNameFormat = "'v'VVV";
            p.SubstituteApiVersionInUrl = true;
        }).AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acquirer Sample Api", Version = "v1" });
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "Acquirer Sample Api", Version = "v2" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

    public static IApplicationBuilder UseApiSettings(this IApplicationBuilder app) =>
        app.UseLoggingMiddleware()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints => { endpoints.MapControllers(); })
            .UseHealthChecks("/health")
            .UseHealthChecks("/health/status",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var executingAssembly = Assembly.GetExecutingAssembly();
                        var informationalVersion = executingAssembly
                            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                            .InformationalVersion;
                        var product = executingAssembly
                            .GetCustomAttribute<AssemblyProductAttribute>()?.Product;
                        var result = $"{product} - {informationalVersion}";

                        context.Response.ContentType = MediaTypeNames.Text.Plain;
                        await context.Response.WriteAsync(result);
                    }
                });

    public static IApplicationBuilder UseApiTools(this IApplicationBuilder app, IConfiguration configuration)
    {
        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        var path = configuration["SwaggerApiSuffix"] ?? string.Empty;

        return app
            .UseSwagger(opt => opt.PreSerializeFilters.Add((doc, req) =>
                doc.Servers = new List<OpenApiServer> { new() { Url = $"{req.Scheme}://{req.Headers.Host}/{path}" } }
            ))
            .UseSwaggerUI(c =>
            {
                apiVersionDescriptionProvider.ApiVersionDescriptions
                    .Select(desc => desc.GroupName)
                    .ToList()
                    .ForEach(groupName =>
                    {
                        c.SwaggerEndpoint($"{groupName}/swagger.json", groupName.ToUpperInvariant());
                    });

                c.DocExpansion(DocExpansion.List);
            });
    }

    public static IApplicationBuilder UseAppAllElasticApm(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.GetValue("ElasticApm:Enabled", false))
            app.UseAllElasticApm(configuration);

        return app;
    }

    public static IServiceCollection ConfigureCompression(this IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = new[] { "application/json" };
            options.Providers.Add<GzipCompressionProvider>();
        });

        return services;
    }

    private static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }
}