using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
	[Route("api/v{version:apiVersion}/ExporterRegistrations")]
	[ApiVersion("1.0")]
	[ApiController]
	public class OtherPermitsController : BaseExporterController<OtherPermitsController, OtherPermitsDto>
	{
		public OtherPermitsController(IOtherPermitsService service, ILogger<OtherPermitsController> logger)
			: base(service, logger)
		{
		}
	}
}
