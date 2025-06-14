using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationService
{
    Task<CreateRegistrationResponseDto> CreateRegistrationAsync(CreateRegistrationDto dto);
    Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto dto);

    Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto dto);
    Task<RegistrationDto?> GetRegistrationByOrganisationAsync(int applicationTypeId, Guid organisationId);
    
    Task<bool> UpdateAsync(int registrationId, UpdateRegistrationDto request);
}