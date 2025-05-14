using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.Api.Handlers;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Epr.Reprocessor.Exporter.Facade.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpClientServiceCollectionExtension
{
    public static IServiceCollection AddServicesAndHttpClients(this IServiceCollection services)
    {
        services.AddTransient<PrnBackendServiceAuthorisationHandler>();

        var PrnServiceApiSettings =
            services.BuildServiceProvider().GetRequiredService<IOptions<PrnBackendServiceApiConfig>>().Value;

        services.AddHttpClient<IRegistrationServiceClient, RegistrationServiceClient>((sp, client) =>
        {
            client.BaseAddress = new Uri(PrnServiceApiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(PrnServiceApiSettings.Timeout);
        })
        .AddHttpMessageHandler<PrnBackendServiceAuthorisationHandler>()
        .AddPolicyHandler(GetRetryPolicy(PrnServiceApiSettings.ServiceRetryCount));

        services.AddHttpClient<IAccreditationServiceClient, AccreditationServiceClient>((sp, client) =>
        {
            client.BaseAddress = new Uri(PrnServiceApiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(PrnServiceApiSettings.Timeout);
        })
        .AddHttpMessageHandler<PrnBackendServiceAuthorisationHandler>()
        .AddPolicyHandler(GetRetryPolicy(PrnServiceApiSettings.ServiceRetryCount));

        services.AddHttpClient<IAccreditationPrnIssueAuthServiceClient, AccreditationPrnIssueAuthServiceClient>((sp, client) =>
        {
            client.BaseAddress = new Uri(PrnServiceApiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(PrnServiceApiSettings.Timeout);
        })
        .AddHttpMessageHandler<PrnBackendServiceAuthorisationHandler>()
        .AddPolicyHandler(GetRetryPolicy(PrnServiceApiSettings.ServiceRetryCount));

        return services;
    }

    private static Polly.Retry.AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}