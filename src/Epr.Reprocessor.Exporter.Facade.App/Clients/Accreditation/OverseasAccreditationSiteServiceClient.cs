using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Extensions;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

public class OverseasAccreditationSiteServiceClient : BaseHttpClient, IOverseasAccreditationSiteServiceClient
{
    private readonly PrnBackendServiceApiConfig config;

    public OverseasAccreditationSiteServiceClient(HttpClient httpClient, IOptions<PrnBackendServiceApiConfig> options)
        : base(httpClient)
    {
        config = options.Value;
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-ORGANISATION", Guid.NewGuid().ToString());
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-USER", Guid.NewGuid().ToString());
    }

    public async Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId)
    {
        var url = string.Format(config.Endpoints.OverseasAccreditationSiteGet, config.ApiVersion, accreditationId);

        return await GetAsync<List<OverseasAccreditationSiteDto>>(url);
    }

    public async Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request)
    {
        var url = string.Format(config.Endpoints.OverseasAccreditationSitePost, config.ApiVersion, accreditationId);

        await PostAsync<OverseasAccreditationSiteDto>(url, request);
    }
}
