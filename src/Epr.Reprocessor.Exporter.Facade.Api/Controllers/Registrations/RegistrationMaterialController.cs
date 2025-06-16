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
    private readonly IRegistrationMaterialService _registrationMaterialService;
    private readonly ILogger<RegistrationMaterialController> _logger;

    public RegistrationMaterialController(IRegistrationMaterialService registrationMaterialService, ILogger<RegistrationMaterialController> logger)
    {
        ArgumentNullException.ThrowIfNull(registrationMaterialService);
        ArgumentNullException.ThrowIfNull(logger);

        _registrationMaterialService = registrationMaterialService;
        _logger = logger;
    }

    [HttpPost("CreateRegistrationMaterial")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
    [SwaggerOperation(
        Summary = "creates a new registration material",
        Description = "attempting to create new registration material"
    )]
    public async Task<IActionResult> CreateRegistrationMaterial([FromBody] CreateRegistrationMaterialRequestDto requestDto)
    {
        if (requestDto == null)
        {
            _logger.LogWarning(LogMessages.InvalidRequest);
            return BadRequest(LogMessages.InvalidRequest);
        }

        _logger.LogInformation(LogMessages.CreateRegistrationMaterial);

        try
        {
            var registrationMaterialId = await _registrationMaterialService.CreateRegistrationMaterial(requestDto);

            return new CreatedResult(string.Empty, registrationMaterialId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }

    [HttpPost("CreateExemptionReferences")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
    [SwaggerOperation(
        Summary = "creates a new exemption references",
        Description = "attempting to create new exemption references"
    )]
    public async Task<IActionResult> CreateExemptionReferences([FromBody] CreateExemptionReferencesDto dto)
    {
        if (dto == null)
        {
            _logger.LogWarning(LogMessages.InvalidRequest);
            return BadRequest(LogMessages.InvalidRequest);
        }

        _logger.LogInformation(LogMessages.CreateExemptionReferences);

        try
        {
            await _registrationMaterialService.CreateExemptionReferences(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }


    [HttpPost("{id:Guid}/permits")]
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
    public async Task<IActionResult> UpdateRegistrationMaterialPermits([FromRoute] Guid id, [FromBody] UpdateRegistrationMaterialPermitsDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationMaterialPermits, id);

        _ = await _registrationMaterialService.UpdateRegistrationMaterialPermitsAsync(id, request);

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

        var result = await _registrationMaterialService.GetMaterialsPermitTypesAsync();

        return Ok(result);
    }

    [HttpGet("{registrationId:guid}/materials")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationMaterialDto>))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "gets existing registration materials associated with a registration.",
        Description = "attempting to get existing registration materials associated with a registration."
    )]
    public async Task<IActionResult> GetAllRegistrationMaterials([FromRoute] Guid registrationId)
    {
        _logger.LogInformation(LogMessages.GetAllRegistrationMaterials, registrationId);

        try
        {
            var materials = await _registrationMaterialService.GetAllRegistrationsMaterials(registrationId);
            return Ok(materials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }
}