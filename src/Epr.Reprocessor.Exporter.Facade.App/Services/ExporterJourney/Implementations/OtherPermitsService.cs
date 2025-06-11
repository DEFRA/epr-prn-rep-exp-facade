using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
    public class OtherPermitsService : BaseReprocessorExporterService<OtherPermitsDto>, IOtherPermitsService
	{
		public OtherPermitsService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options) : base(apiClient, options)
		{
			BaseGetUrl = string.Format(Config.ExportEndpoints.OtherPermitsGet, Config.ApiVersion);
			BasePostUrl = string.Format(Config.ExportEndpoints.OtherPermitsPost, Config.ApiVersion);
		}
	}
}
