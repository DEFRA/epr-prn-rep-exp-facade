using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationServiceClient
{
    Task<int> CreateRegistrationAsync(CreateRegistrationDto request);
    Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto request);
    Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto request);
    Task<RegistrationDto> GetRegistrationByOrganisationAsync(int applicationTypeId, int organisationId);
    Task<bool> UpdateAsync(int registrationId, UpdateRegistrationDto request);
}
