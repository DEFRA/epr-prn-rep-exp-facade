using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationMaterialService(IRegistrationMaterialServiceClient registrationMaterialServiceClient) : IRegistrationMaterialService
{
    public async Task CreateExemptionReferences(CreateExemptionReferencesDto dto)
        => await registrationMaterialServiceClient.CreateExemptionReferencesAsync(dto);

    public async Task<int> CreateRegistrationMaterial(CreateRegistrationMaterialDto dto)
        => await registrationMaterialServiceClient.CreateRegistrationMaterialAsync(dto);

}
