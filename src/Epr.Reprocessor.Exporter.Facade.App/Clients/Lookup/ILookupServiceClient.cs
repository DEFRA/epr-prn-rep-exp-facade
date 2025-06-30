
namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Lookup;

public interface ILookupServiceClient
{
    Task<IEnumerable<string>> GetCountries();
}