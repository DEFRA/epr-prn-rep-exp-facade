using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationService(IRegistrationServiceClient registrationServiceClient) : IRegistrationService
{
    public async Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto dto)
    {
        return await registrationServiceClient.UpdateSiteAddressAsync(registrationId, dto);
    }

    public async Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto dto)
    {
        return await registrationServiceClient.UpdateRegistrationTaskStatusAsync(registrationId, dto);
    }
}
