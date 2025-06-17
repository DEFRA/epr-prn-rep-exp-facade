using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
    public interface IOtherPermitsService
	{
		Task<OtherPermitsDto> Get(Guid registrationId);

		Task<Guid> Create(Guid registrationId, OtherPermitsDto value);

		Task<bool> Update(Guid registrationId, OtherPermitsDto value);
	}
}
