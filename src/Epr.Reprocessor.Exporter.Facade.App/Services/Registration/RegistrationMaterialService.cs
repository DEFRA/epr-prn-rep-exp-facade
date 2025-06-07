using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationMaterialService(IRegistrationMaterialServiceClient registrationMaterialServiceClient) : IRegistrationMaterialService
{
    public async Task CreateRegistrationMaterialAndExemptionReferences(CreateRegistrationMaterialAndExemptionReferencesDto dto)
        => await registrationMaterialServiceClient.CreateRegistrationMaterialAndExemptionReferencesAsync(dto);
}
