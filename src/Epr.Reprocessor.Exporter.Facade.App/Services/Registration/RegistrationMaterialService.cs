using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationMaterialService(IRegistrationMaterialServiceClient registrationMaterialServiceClient) : IRegistrationMaterialService
{
    public async Task CreateExemptionReferences(CreateExemptionReferencesDto dto)
        => await registrationMaterialServiceClient.CreateExemptionReferencesAsync(dto);

    public async Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterial(CreateRegistrationMaterialRequestDto requestDto)
        => await registrationMaterialServiceClient.CreateRegistrationMaterialAsync(requestDto);


    public async Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid id, UpdateRegistrationMaterialPermitsDto request)
    {
        return await registrationMaterialServiceClient.UpdateRegistrationMaterialPermitsAsync(id, request);
    }

    public async Task<bool> UpdateRegistrationMaterialPermitCapacityAsync(Guid id, UpdateRegistrationMaterialPermitCapacityDto request)
    {
        return await registrationMaterialServiceClient.UpdateRegistrationMaterialPermitCapacityAsync(id, request);
    }

    public async Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync()
    {
        return await registrationMaterialServiceClient.GetMaterialsPermitTypesAsync();
    }

    public async Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationsMaterials(Guid registrationId)
        => await registrationMaterialServiceClient.GetAllRegistrationMaterialsAsync(registrationId);
}