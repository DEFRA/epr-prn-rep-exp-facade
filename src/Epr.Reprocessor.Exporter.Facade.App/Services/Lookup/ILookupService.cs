namespace Epr.Reprocessor.Exporter.Facade.App.Services.Lookup;

public interface ILookupService
{
    Task<IEnumerable<string>> GetCountries();
}