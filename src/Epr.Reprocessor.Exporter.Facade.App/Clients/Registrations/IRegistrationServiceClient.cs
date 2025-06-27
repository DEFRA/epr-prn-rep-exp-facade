using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationServiceClient
{
    Task<CreateRegistrationResponseDto> CreateRegistrationAsync(CreateRegistrationDto request);
    Task<bool> UpdateRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto request);
    Task<bool> UpdateApplicantRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto request);
    Task<bool> UpdateSiteAddressAsync(Guid registrationId, UpdateRegistrationSiteAddressDto request);
    Task<RegistrationDto?> GetRegistrationByOrganisationAsync(int applicationTypeId, Guid organisationId);
    Task<bool> UpdateAsync(Guid registrationId, UpdateRegistrationDto request);
    Task<RegistrationOverviewDto> GetRegistrationOverviewAsync(Guid registrationId);
}