using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
    [Route("api/v{version:apiVersion}/ExporterRegistrations")]
	[ApiVersion("1.0")]
	[ApiController]
	public class AddressForServiceOfNoticesController : Controller
	{
		private readonly IAddressForServiceOfNoticesService _service;
		private readonly ILogger<AddressForServiceOfNoticesController> _logger;

		public AddressForServiceOfNoticesController(IAddressForServiceOfNoticesService service, ILogger<AddressForServiceOfNoticesController> logger)
		{
			ArgumentNullException.ThrowIfNull(service);
			ArgumentNullException.ThrowIfNull(logger);

			_service = service;
			_logger = logger;
		}

        [HttpGet("{registrationId:Guid}/address-for-service-of-notices")]
        [ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get(Guid registrationId)
		{
			_logger.LogInformation($"Get AddressForServiceOfNotices for registrationId: {registrationId}");

            var dto = await _service.Get(registrationId);
			return dto == null ? NotFound() : Ok(dto);
		}

        [HttpPut("{registrationId:Guid}/address-for-service-of-notices")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(Guid registrationId, [FromBody] AddressForServiceOfNoticesDto value)
		{
            _logger.LogInformation($"Update AddressForServiceOfNotices for registrationId: {value.RegistrationId}");

            await _service.Update(registrationId, value);
			return Accepted();
		}
	}
}
