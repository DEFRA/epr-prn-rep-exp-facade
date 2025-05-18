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

    public async Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto request)
    {
        logger.LogInformation("UpdateRegistrationTaskStatusAsync for Registration ID: {0}", registrationId);

        // e.g. api/v{0}/registrations/{1}/siteAddress
        var url = string.Format(_config.Endpoints.RegistrationUpdateTaskStatus, _config.ApiVersion, registrationId);

        return await this.PostAsync<UpdateRegistrationTaskStatusDto, bool>(url, request);
    }

    public async Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto request)
    {
        logger.LogInformation("UpdateSiteAddressAsync for Registration ID: {0}", registrationId);

        // e.g. api/v{0}/registrations/{1}/siteAddress
        var url = string.Format(_config.Endpoints.RegistrationUpdateSiteAddress, _config.ApiVersion, registrationId);
        
        return await this.PostAsync<UpdateRegistrationSiteAddressDto, bool>(url, request);
    }
}