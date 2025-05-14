using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public interface IRegistrationServiceClient
{
    Task<bool> UpdateSiteAddress(int registrationId, UpdateSiteAddressDto request);
}
