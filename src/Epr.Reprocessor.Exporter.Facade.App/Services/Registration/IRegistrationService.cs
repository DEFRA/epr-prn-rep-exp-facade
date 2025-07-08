using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationService
{
    Task<CreateRegistrationResponseDto> CreateRegistrationAsync(CreateRegistrationDto dto);
    Task<bool> UpdateRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto dto);
    Task<bool> UpdateApplicantRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto dto);
    Task<bool> UpdateSiteAddressAsync(Guid registrationId, UpdateRegistrationSiteAddressDto dto);
    Task<RegistrationDto?> GetRegistrationByOrganisationAsync(int applicationTypeId, Guid organisationId);   
    Task<bool> UpdateAsync(Guid registrationId, UpdateRegistrationDto request); 
    Task<ApplicantRegistrationTaskOverviewDto> GetRegistrationOverviewAsync(Guid registrationId);
    Task<IEnumerable<RegistrationsOverviewDto>> GetRegistrationsOverviewByOrgIdAsync(Guid organisationId);
}