using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

[Route("api/v{version:apiVersion}/registrationMaterials")]
[ApiVersion("1.0")]
[ApiController]
public class ExporterController : ControllerBase
{
    private readonly IRegistrationMaterialService _registrationMaterialService;
    private readonly ILogger<ExporterController> _logger;

    public ExporterController(IRegistrationMaterialService registrationMaterialService, ILogger<ExporterController> logger)
    {
        ArgumentNullException.ThrowIfNull(registrationMaterialService);
        ArgumentNullException.ThrowIfNull(logger);
        this._registrationMaterialService = registrationMaterialService;
        this._logger = logger;
    }

    [HttpPost("SaveOverseasReprocessor")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
    [SwaggerOperation(
        Summary = "creates or updates a overseas reprocessor",
        Description = "attempting to create or update overseas reprocessor"
    )]
    public async Task<IActionResult> SaveOverseasReprocessor([FromBody] OverseasAddressRequest request)
    {
        if (request == null)
        {
            _logger.LogWarning(LogMessages.InvalidRequest);
            return BadRequest(LogMessages.InvalidRequest);
        }

        _logger.LogInformation(LogMessages.CreateRegistrationMaterial);

        try
        {
            await _registrationMaterialService.SaveOverseasReprocessorAsync(request);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }
}
