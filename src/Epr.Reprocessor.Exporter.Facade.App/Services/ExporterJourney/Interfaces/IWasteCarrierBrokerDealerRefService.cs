using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
    public interface IWasteCarrierBrokerDealerRefService
    {
        Task<WasteCarrierBrokerDealerRefDto> Get(int registrationId);

        Task<int> Create(int registrationId, WasteCarrierBrokerDealerRefDto value);

        Task<bool> Update(int registrationId, WasteCarrierBrokerDealerRefDto value);
    }
}
