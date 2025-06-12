using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
	[Route("api/v{version:apiVersion}/ExporterRegistrations/other-permits")]
	[ApiVersion("1.0")]
	[ApiController]
	public class OtherPermitsController : Controller
	{
		private readonly IOtherPermitsService _service;
		private readonly ILogger<OtherPermitsController> _logger;

		public OtherPermitsController(IOtherPermitsService service, ILogger<OtherPermitsController> logger)
		{
			ArgumentNullException.ThrowIfNull(service);
			ArgumentNullException.ThrowIfNull(logger);

			_service = service;
			_logger = logger;
		}

		[HttpGet("{registrationId}")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get(int registrationId)
		{
			var dto = await _service.Get(registrationId);
			return dto == null ? NotFound() : Ok(dto);
		}

		[HttpPost]
		public async virtual Task<IActionResult> Post([FromBody] OtherPermitsDto value)
		{
			var result = await _service.Create(value);
			return Ok(result);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(int id, [FromBody] OtherPermitsDto value)
		{
			await _service.Update(id, value);
			return Accepted();
		}
	}
}
