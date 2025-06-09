using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
	public class OtherPermitsService<OtherPermitsInDto, OtherPermitsOutDto> : BaseExporterService<OtherPermitsInDto, OtherPermitsOutDto>
	{
		public OtherPermitsService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options) : base(apiClient, options)
		{
			BaseGetUrl = string.Format(Config.ExportEndpoints.OtherPermitsGet, Config.ApiVersion);
			BasePostUrl = string.Format(Config.ExportEndpoints.OtherPermitsPost, Config.ApiVersion);
		}
	}
}
