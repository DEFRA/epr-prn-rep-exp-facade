using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationMaterialServiceClient
{
    Task CreateExemptionReferencesAsync(CreateExemptionReferencesDto request);
    Task<Guid> CreateRegistrationMaterialAsync(CreateRegistrationMaterialDto request);
}
