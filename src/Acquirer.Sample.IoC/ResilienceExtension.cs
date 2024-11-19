using Acquirer.Sample.IoC.Options;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;
using System.Text;

namespace Acquirer.Sample.IoC;

public static class ResilienceExtension
{
    public static IHttpClientBuilder ConfigureResilience(this IHttpClientBuilder builder, ResilienceOptions resilienceOptions)
    {
        if (!resilienceOptions.Enabled)
            return builder;

        // policies
        var retryPolicy = GetRetryPolicy(resilienceOptions);
        var timeoutPolicy = GetTimeoutPolicy(resilienceOptions);
        var fallbackTimeout = GetFallbackPolicy<TimeoutRejectedException>();

        // wraps
        var timeoutWrap = fallbackTimeout.WrapAsync(timeoutPolicy);
        var policies = retryPolicy.WrapAsync(timeoutWrap);

        // builder
        builder
            .AddPolicyHandler(policies);

        return builder;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ResilienceOptions resilienceOptions)
    {
        var maxRetryAttempts = resilienceOptions.MaxRetryAttempts ?? 5;
        var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), maxRetryAttempts);

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(delay, (exception, timeSpan, retries, context) =>
            {
                if (maxRetryAttempts != retries)
                    return;

                Console.Out.WriteLine($"Polly timeout: Get timeout with {retries} retries, total delay: {timeSpan}.");
            });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(ResilienceOptions resilienceOptions)
    {
        var timeout = resilienceOptions.Timeout ?? 30;
        return Policy.TimeoutAsync<HttpResponseMessage>(timeout, TimeoutStrategy.Optimistic);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy<TException>() where TException : Exception
    {
        return Policy<HttpResponseMessage>
            .Handle<TException>()
            .FallbackAsync(FallbackAction, OnFallbackAsync);
    }

    private static Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
    {
        Console.Out.WriteLine($"Polly fallback started.");
        return Task.CompletedTask;
    }

    private static Task<HttpResponseMessage> FallbackAction(
        DelegateResult<HttpResponseMessage> responseToFailedRequest,
        Context context,
        CancellationToken cancellationToken)
    {
        Console.Out.WriteLine("Fallback action is executing");
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            RequestMessage = new HttpRequestMessage(),
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };
        return Task.FromResult(httpResponseMessage);
    }
}
