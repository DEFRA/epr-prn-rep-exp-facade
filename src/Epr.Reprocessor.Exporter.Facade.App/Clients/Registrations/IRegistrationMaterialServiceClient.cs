namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationMaterialServiceClient
{
    Task CreateExemptionReferencesAsync(CreateExemptionReferencesDto request);
    Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterialAsync(CreateRegistrationMaterialRequestDto request);

    Task<List<RegistrationMaterialDto>> GetAllRegistrationMaterialsAsync(Guid registrationId);
}