using Epr.Reprocessor.Exporter.Facade.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney
{
    public class BaseExporterController<TController, TDto> : Controller
	{
		protected readonly IBaseReprocessorExporterService<TDto> ExporterService;
		protected readonly ILogger<TController> _logger;

		public BaseExporterController(IBaseReprocessorExporterService<TDto> exporterService, ILogger<TController> logger)
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
		public async virtual Task<IActionResult> Post([FromBody] TDto value)
		{
			await ExporterService.Create(value);
			return Ok();
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		public async virtual Task<IActionResult> Put(int id, [FromBody] TDto value)
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
