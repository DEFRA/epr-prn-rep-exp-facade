using Epr.Reprocessor.Exporter.Facade.App.Clients.Lookup;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Lookup;

public class LookupService(ILookupServiceClient lookupServiceClient) : ILookupService
{
    public async Task<IEnumerable<string>> GetCountries()
    {
        return await lookupServiceClient.GetCountries();
    }
}