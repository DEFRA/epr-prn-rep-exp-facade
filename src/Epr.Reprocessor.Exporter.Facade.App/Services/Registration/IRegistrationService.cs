using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationService
{
    Task<bool> UpdateSiteAddress(int registrationId, UpdateSiteAddressDto dto);
}
