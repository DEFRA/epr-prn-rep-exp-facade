using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Lookup;

public class LookupServiceClient(
HttpClient httpClient,
IOptions<PrnBackendServiceApiConfig> options)
: BaseHttpClient(httpClient), ILookupServiceClient
{
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    public async Task<IEnumerable<string>> GetCountries()
    {
        var url = string.Format(Endpoints.Lookup.Countries, _config.ApiVersion);

        return await GetAsync<IEnumerable<string>>(url);
    }
}