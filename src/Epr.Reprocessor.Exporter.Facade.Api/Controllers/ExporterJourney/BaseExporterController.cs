using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
	public class BaseExporterController<TController, TDtoIn, TDtoOut> : Controller
	{
		protected readonly IExporterService<TDtoIn, TDtoOut> ExporterService;
		protected readonly ILogger<TController> _logger;

		public BaseExporterController(IExporterService<TDtoIn, TDtoOut> exporterService, ILogger<TController> logger)
		{
			ArgumentNullException.ThrowIfNull(exporterService);
			ArgumentNullException.ThrowIfNull(logger);

			ExporterService = exporterService;
			_logger = logger;
		}

		[HttpGet]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get()
		{
			var dtos = await ExporterService.Get();
			return dtos == null ? NotFound() : Ok(dtos);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async virtual Task<IActionResult> Get<TOut>(int id)
		{
			var dto = await ExporterService.Get(id);
			return dto == null ? NotFound() : Ok(dto);
		}

		[HttpPost]
		public async virtual Task<IActionResult> Post([FromBody] TDtoIn value)
		{
			await ExporterService.Create(value);
			return Ok();
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(int id, [FromBody] TDtoIn value)
		{
			await ExporterService.Create(value);
			return Accepted();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Delete(int id)
		{
			await ExporterService.Delete(id);
			return Accepted();

		}
	}
}
