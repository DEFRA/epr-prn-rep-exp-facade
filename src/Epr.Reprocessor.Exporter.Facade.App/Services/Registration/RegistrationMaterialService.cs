using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
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

    public async Task<bool> UpdateIsMaterialRegisteredAsync(List<UpdateIsMaterialRegisteredDto> request)
        => await registrationMaterialServiceClient.UpdateIsMaterialRegisteredAsync(request);

    public async Task<RegistrationMaterialContactDto> UpsertRegistrationMaterialContactAsync(Guid registrationMaterialId,
        RegistrationMaterialContactDto request)
        => await registrationMaterialServiceClient.UpsertRegistrationMaterialContactAsync(registrationMaterialId, request);

    public async Task UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIORequestDto request)
        => await registrationMaterialServiceClient.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialId, request);

    public async Task<bool> SaveOverseasReprocessorAsync(OverseasAddressRequest request, Guid createdBy)
        => await registrationMaterialServiceClient.SaveOverseasReprocessorAsync(request, createdBy);

    public async Task<bool> UpdateMaximumWeight(Guid registrationMaterialId, UpdateMaximumWeightDto request)
        => await registrationMaterialServiceClient.UpdateMaximumWeightAsync(registrationMaterialId, request);

    public async Task UpdateMaterialNotReprocessingReasonAsync(Guid registrationMaterialId, string materialNotReprocessingReason)
        => await registrationMaterialServiceClient.UpdateMaterialNotReprocessingReasonAsync(registrationMaterialId, materialNotReprocessingReason);
}