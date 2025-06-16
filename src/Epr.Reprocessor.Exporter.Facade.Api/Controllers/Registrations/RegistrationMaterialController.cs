using System.Net;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

[Route("api/v{version:apiVersion}/registrationMaterials")]
[ApiVersion("1.0")]
[ApiController]
public class RegistrationMaterialController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<RegistrationMaterialController> _logger;

    public RegistrationMaterialController(IRegistrationService registrationService, ILogger<RegistrationMaterialController> logger)
    {
        ArgumentNullException.ThrowIfNull(registrationService);
        ArgumentNullException.ThrowIfNull(logger);

        _registrationService = registrationService;
        _logger = logger;
    }

    [HttpPost("{externalId:Guid}/permits")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "updates an existing registration material permits",
        Description = "attempting to update the registration material permits."
    )]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns No Content")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration is not found", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateRegistrationMaterialPermits([FromRoute] Guid externalId, [FromBody] UpdateRegistrationMaterialPermitsDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationMaterialPermits, externalId);

        _ = await _registrationService.UpdateRegistrationMaterialPermitsAsync(externalId, request);

        return NoContent();
    }

    [HttpGet("permitTypes")]
    [ProducesResponseType(typeof(List<IdNamePairDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ContentResult), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieves list of material permit types",
        Description = "Returns a list of material permit types used during registration."
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved permit types.", typeof(List<MaterialsPermitTypeDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred.", typeof(ContentResult))]
    public async Task<IActionResult> GetMaterialsPermitTypes()
    {
        _logger.LogInformation(LogMessages.GetMaterialsPermitTypes);

        var result = await _registrationService.GetMaterialsPermitTypesAsync();

        return Ok(result);
    }
}
