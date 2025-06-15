using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationMaterialService(IRegistrationMaterialServiceClient registrationMaterialServiceClient) : IRegistrationMaterialService
{
    public async Task CreateExemptionReferences(CreateExemptionReferencesDto dto)
        => await registrationMaterialServiceClient.CreateExemptionReferencesAsync(dto);

    public async Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterial(CreateRegistrationMaterialRequestDto requestDto)
        => await registrationMaterialServiceClient.CreateRegistrationMaterialAsync(requestDto);

    public async Task<List<RegistrationMaterialDto>> GetAllRegistrationsMaterials(Guid registrationId)
        => await registrationMaterialServiceClient.GetAllRegistrationMaterialsAsync(registrationId);
}