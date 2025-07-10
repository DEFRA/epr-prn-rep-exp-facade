using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationMaterialServiceClient
{
    Task CreateExemptionReferencesAsync(CreateExemptionReferencesDto request);
    Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterialAsync(CreateRegistrationMaterialRequestDto request);
    Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid id, UpdateRegistrationMaterialPermitsDto request);
    Task<bool> UpdateRegistrationMaterialPermitCapacityAsync(Guid id, UpdateRegistrationMaterialPermitCapacityDto request);
    Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync();
    Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationMaterialsAsync(Guid registrationId);
    Task<bool> DeleteAsync(Guid registrationMaterialId);
	Task<bool> UpdateIsMaterialRegisteredAsync(List<UpdateIsMaterialRegisteredDto> request);
    Task<RegistrationMaterialContactDto> UpsertRegistrationMaterialContactAsync(Guid registrationMaterialId, RegistrationMaterialContactDto request);
    Task UpsertRegistrationReprocessingDetailsAsync(Guid registrationMaterialId, RegistrationReprocessingIORequestDto request);
    Task<bool> SaveOverseasReprocessorAsync(OverseasAddressRequest requestDto, Guid createdBy);
    Task<List<OverseasMaterialReprocessingSiteDto>> GetOverseasMaterialReprocessingSites(Guid registrationMaterialId);
    Task SaveInterimSitesAsync(SaveInterimSitesRequestDto requestDto, Guid createdBy);
}
