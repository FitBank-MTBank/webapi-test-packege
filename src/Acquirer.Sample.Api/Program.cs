using Elastic.Apm.NetCoreAll;
using Acquirer.Sample.Api.Extensions;
using Acquirer.Sample.Infrastructure.Extensions;
using Acquirer.Sample.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureDbContext(builder.Configuration)
    .RegisterServices(builder.Configuration)
    .AddRepositories()
    .AddApiSettings()
    .AddApiTools()
    .ConfigureCompression();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Host
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureLogging(logging => { logging.ClearProviders(); })
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.WebHost
    .ConfigureKestrel(options => { options.AddServerHeader = false; })
    .UseIIS();

var app = builder.Build();

app.UseSerilogRequestLogging()
    .UseAllElasticApm(app.Configuration)
    .UseApiSettings()
    .UseApiTools(app.Configuration)
    .UseResponseCompression();

app.UseExceptionHandler();

app.Run();

namespace Acquirer.Sample.Api
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        protected Program() { }
    }
}