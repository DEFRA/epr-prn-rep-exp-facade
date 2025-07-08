using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;

public interface IWasteCarrierBrokerDealerRefService
{
    Task<WasteCarrierBrokerDealerRefDto> Get(Guid registrationId);

    Task<Guid> Create(Guid registrationId, WasteCarrierBrokerDealerRefDto value);

    Task<bool> Update(Guid registrationId, WasteCarrierBrokerDealerRefDto value);
}