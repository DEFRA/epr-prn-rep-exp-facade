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

    public async Task<bool> UpdateSiteAddressAndContactDetails(UpdateSiteAddressAndContactDetailsDto request)
    {
        logger.LogInformation("UpdateSiteAddressAndContactDetails for ID: {0}", request.Id);

        // e.g. api/v{0}/registrations/siteaddressandcontactdetails
        var url = string.Format(_config.Endpoints.UpdateSiteAddressAndContactDetails, _config.ApiVersion);
        
        return await this.PostAsync<UpdateSiteAddressAndContactDetailsDto, bool>(url, request);
    }
}