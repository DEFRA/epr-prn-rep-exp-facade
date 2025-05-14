using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationService(IRegistrationServiceClient registrationServiceClient) : IRegistrationService
{
    public async Task<bool> UpdateSiteAddress(int registrationId, UpdateSiteAddressDto dto)
    {
        return await registrationServiceClient.UpdateSiteAddress(registrationId, dto);
    }
}
