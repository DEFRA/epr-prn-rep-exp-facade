using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
    [Route("api/v{version:apiVersion}/ExporterRegistrations")]
	[ApiVersion("1.0")]
	[ApiController]
	public class WasteCarrierBrokerDealerRefController : Controller
	{
		private readonly IWasteCarrierBrokerDealerRefService _service;
		private readonly ILogger<WasteCarrierBrokerDealerRefController> _logger;

		public WasteCarrierBrokerDealerRefController(IWasteCarrierBrokerDealerRefService service, ILogger<WasteCarrierBrokerDealerRefController> logger)
		{
			ArgumentNullException.ThrowIfNull(service);
			ArgumentNullException.ThrowIfNull(logger);

			_service = service;
			_service = service;
			_logger = logger;
		}

        [HttpGet("{registrationId:Guid}/waste-carrier-broker-dealer-ref")]
        [ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get(Guid registrationId)
		{
			_logger.LogInformation($"Get WasteCarrierBrokerDealerRef for registrationId: {registrationId}");

            var dto = await _service.Get(registrationId);
			return dto == null ? NotFound() : Ok(dto);
		}

		[HttpPost]		
		public async virtual Task<IActionResult> Post([FromBody] WasteCarrierBrokerDealerRefDto value)
		{
			_logger.LogInformation($"Create WasteCarrierBrokerDealerRef for registrationId: {value.RegistrationId}");

            var result = await _service.Create(value.RegistrationId, value);
			return Ok(result);
		}

        [HttpPut("{registrationId:Guid}/waste-carrier-broker-dealer-ref")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(Guid registrationId, [FromBody] WasteCarrierBrokerDealerRefDto value)
		{
            _logger.LogInformation($"Update WasteCarrierBrokerDealerRef for registrationId: {value.RegistrationId}");

            await _service.Update(registrationId, value);
			return Accepted();
		}
	}
}
