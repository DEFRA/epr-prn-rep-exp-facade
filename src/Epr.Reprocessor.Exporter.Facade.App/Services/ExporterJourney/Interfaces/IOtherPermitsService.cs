using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
    public interface IOtherPermitsService
	{
		Task<CarrierBrokerDealerPermitsDto> Get(Guid registrationId);

		Task<Guid> Create(Guid registrationId, CarrierBrokerDealerPermitsDto value);

		Task<bool> Update(Guid registrationId, CarrierBrokerDealerPermitsDto value);
	}
}
