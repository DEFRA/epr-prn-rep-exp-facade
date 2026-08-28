using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
    public interface IAddressForServiceOfNoticesService
    {
        Task<AddressForServiceOfNoticesDto> Get(Guid registrationId);

        Task<bool> Update(Guid registrationId, AddressForServiceOfNoticesDto value);
    }
}
