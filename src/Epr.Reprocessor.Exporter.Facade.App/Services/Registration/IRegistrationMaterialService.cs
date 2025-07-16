using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationMaterialService
{
    Task CreateExemptionReferences(CreateExemptionReferencesDto dto);

    Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterial(CreateRegistrationMaterialRequestDto requestDto);

    Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid externalId, UpdateRegistrationMaterialPermitsDto request);

    Task<bool> UpdateRegistrationMaterialPermitCapacityAsync(Guid id, UpdateRegistrationMaterialPermitCapacityDto request);

    Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync();

    Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationsMaterials(Guid registrationId);
    Task<bool> Delete(Guid registrationMaterialId);

	Task<bool> UpdateIsMaterialRegisteredAsync(List<UpdateIsMaterialRegisteredDto> request);

    Task<RegistrationMaterialContactDto> UpsertRegistrationMaterialContactAsync(Guid registrationMaterialId, RegistrationMaterialContactDto request);

    Task UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIORequestDto request);

    Task<bool> SaveOverseasReprocessorAsync(OverseasAddressRequest request, Guid createdBy);

    Task<bool> UpdateMaximumWeight(Guid registrationMaterialId, UpdateMaximumWeightDto request);
    Task<bool> SaveOverseasReprocessorAsync(OverseasAddressRequest requestDto, Guid createdBy);
    Task<List<OverseasMaterialReprocessingSiteDto>> GetOverseasMaterialReprocessingSites(Guid registrationMaterialId);
    Task SaveInterimSitesAsync(SaveInterimSitesRequestDto requestDto, Guid createdBy);
}
