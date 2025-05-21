using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public class RegistrationServiceClient(
HttpClient httpClient,
IOptions<PrnBackendServiceApiConfig> options,
ILogger<RegistrationServiceClient> logger)
: BaseHttpClient(httpClient), IRegistrationServiceClient
{
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    [ExcludeFromCodeCoverage(Justification = "TODO: Unit tests to be added as part of create registration user story")]
    public async Task<int> CreateRegistrationAsync(CreateRegistrationDto request)
    {
        logger.LogInformation("CreateRegistrationAsync for ApplicationTypeId ID: {ApplicationTypeId}", request.ApplicationTypeId);

        // e.g. api/v{0}/registrations
        var url = string.Format(_config.Endpoints.CreateRegistration, _config.ApiVersion);

        return await this.PostAsync<CreateRegistrationDto, int>(url, request);
    }

    public async Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto request)
    {
        logger.LogInformation("UpdateRegistrationTaskStatusAsync for Registration ID: {RegistrationId}", registrationId);

        // e.g. api/v{0}/registrations/{1}/siteAddress
        var url = string.Format(_config.Endpoints.RegistrationUpdateTaskStatus, _config.ApiVersion, registrationId);

        return await this.PostAsync<UpdateRegistrationTaskStatusDto, bool>(url, request);
    }

    public async Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto request)
    {
        logger.LogInformation("UpdateSiteAddressAsync for Registration ID: {RegistrationId}", registrationId);

        // e.g. api/v{0}/registrations/{1}/siteAddress
        var url = string.Format(_config.Endpoints.RegistrationUpdateSiteAddress, _config.ApiVersion, registrationId);
        
        return await this.PostAsync<UpdateRegistrationSiteAddressDto, bool>(url, request);
    }
}