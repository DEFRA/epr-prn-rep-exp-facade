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
    Task<bool> UpdateMaximumWeightAsync(Guid registrationMaterialId, UpdateMaximumWeightDto request);
    Task<List<GetMaterialExemptionReferenceDto>> GetMaterialExemptionReferenceAsync(Guid materialRegistrationId);
}
