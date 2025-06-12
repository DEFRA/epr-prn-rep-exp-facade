using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

namespace Epr.Reprocessor.Exporter.Facade.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static void RegisterComponents(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterConfigs(services, configuration);
        RegisterServices(services, configuration);
    }

    private static void RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PrnBackendServiceApiConfig>(configuration.GetSection(PrnBackendServiceApiConfig.SectionName));
    }

    private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Service Clients
        services.AddScoped<IRegistrationServiceClient, RegistrationServiceClient>();
        services.AddScoped<IAccreditationServiceClient, AccreditationServiceClient>();
        services.AddScoped<IAccreditationPrnIssueAuthServiceClient, AccreditationPrnIssueAuthServiceClient>();

        // Services
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IAccreditationService, AccreditationService>();
        services.AddScoped<IAccreditationPrnIssueAuthService, AccreditationPrnIssueAuthService>();

        // Services (Export journey)
		services.AddScoped<IOtherPermitsService, OtherPermitsService>();
	}
}
