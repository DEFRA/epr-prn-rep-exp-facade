using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationMaterialService
{
    Task CreateExemptionReferences(CreateExemptionReferencesDto dto);
}
