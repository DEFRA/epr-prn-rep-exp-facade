using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Services.Lookup;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Lookup;

[Route("api/v{version:apiVersion}/Lookup")]
[ApiVersion("1.0")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ILookupService _LookupService;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ILookupService lookupService, ILogger<CountryController> logger)
    {
        ArgumentNullException.ThrowIfNull(lookupService);
        ArgumentNullException.ThrowIfNull(logger);

        _LookupService = lookupService;
        _logger = logger;
    }

    [HttpGet("countries")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        _logger.LogInformation(LogMessages.GetCountries);

        var countries = await _LookupService.GetCountries();
        return Ok(countries);
    }
}