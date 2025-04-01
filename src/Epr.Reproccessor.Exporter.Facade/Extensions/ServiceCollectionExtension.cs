using Epr.Reproccessor.Exporter.Facade.App.Config;

namespace Epr.Reproccessor.Exporter.Facade.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterComponents(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterConfigs(services, configuration);
            RegisterServices(services, configuration);
        }

        private static void RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AccountsServiceApiConfig>(configuration.GetSection(AccountsServiceApiConfig.SectionName));
            services.Configure<PrnBackendServiceApiConfig>(configuration.GetSection(PrnBackendServiceApiConfig.SectionName));

        }

        private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
