using Epr.Reproccessor.Exporter.Facade.Api.Services;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using Epr.Reproccessor.Exporter.Facade.App.Config;
using Epr.Reproccessor.Exporter.Facade.Handlers;
using Microsoft.Extensions.Options;

namespace Epr.Reproccessor.Exporter.Facade.Api.Extensions
{
    public static class HttpClientServiceCollectionExtension
    {
        public static IServiceCollection AddServicesAndHttpClients(this IServiceCollection services)
        {
            services.AddTransient<AccountServiceAuthorisationHandler>();

            services.AddHttpClient<ISaveAndContinueService, SaveAndContinueService>((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<PrnBackendServiceApiConfig>>().Value;

                client.BaseAddress = new Uri(config.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(config.Timeout);
            })
           .AddHttpMessageHandler<AccountServiceAuthorisationHandler>();

            return services;
        }
    }
}
