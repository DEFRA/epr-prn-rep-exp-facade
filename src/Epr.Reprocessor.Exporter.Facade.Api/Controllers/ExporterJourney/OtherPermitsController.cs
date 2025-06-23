using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
    [Route("api/v{version:apiVersion}/ExporterRegistrations")]
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

        [HttpGet("{registrationId:Guid}/carrier-broker-dealer-permits")]
        [ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get(Guid registrationId)
		{
			var dto = await _service.Get(registrationId);
			return dto == null ? NotFound() : Ok(dto);
		}

        /// <summary>
        /// A [CarrierBrokerDealerPermits] record can only be created by using the [WasteCarrierBrokerDealerRef] controller.
		/// This method should be used to update individual data points within the record.
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("{registrationId:Guid}/carrier-broker-dealer-permits")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(Guid registrationId, [FromBody] CarrierBrokerDealerPermitsDto value)
		{
			await _service.Update(registrationId, value);
			return Accepted();
		}
	}
}
