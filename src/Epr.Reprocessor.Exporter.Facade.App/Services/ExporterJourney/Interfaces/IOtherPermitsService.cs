using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
    public interface IOtherPermitsService
	{
		Task<OtherPermitsDto> Get(int registrationId);

		Task<int> Create(int registrationId, OtherPermitsDto value);

		Task<bool> Update(int registrationId, OtherPermitsDto value);
	}
}
