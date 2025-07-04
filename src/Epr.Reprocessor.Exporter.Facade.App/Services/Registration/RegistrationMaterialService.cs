﻿using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationMaterialService(IRegistrationMaterialServiceClient registrationMaterialServiceClient) : IRegistrationMaterialService
{
    public async Task CreateExemptionReferences(CreateExemptionReferencesDto dto)
        => await registrationMaterialServiceClient.CreateExemptionReferencesAsync(dto);

    public async Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterial(CreateRegistrationMaterialRequestDto requestDto)
        => await registrationMaterialServiceClient.CreateRegistrationMaterialAsync(requestDto);


    public async Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid externalId, UpdateRegistrationMaterialPermitsDto request)
        => await registrationMaterialServiceClient.UpdateRegistrationMaterialPermitsAsync(externalId, request);

    public async Task<bool> UpdateRegistrationMaterialPermitCapacityAsync(Guid id, UpdateRegistrationMaterialPermitCapacityDto request)
        => await registrationMaterialServiceClient.UpdateRegistrationMaterialPermitCapacityAsync(id, request);

    public async Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync()
        => await registrationMaterialServiceClient.GetMaterialsPermitTypesAsync();

    public async Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationsMaterials(Guid registrationId)
        => await registrationMaterialServiceClient.GetAllRegistrationMaterialsAsync(registrationId);

    public async Task<bool> Delete(Guid registrationMaterialId) 
        => await registrationMaterialServiceClient.DeleteAsync(registrationMaterialId);
}