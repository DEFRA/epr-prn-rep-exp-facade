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
            _logger.LogError(ex, LogMessages.UnExpectedError);
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
            _logger.LogError(ex, LogMessages.UnExpectedError);
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

    [HttpPost("{id:Guid}/permitCapacity")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [SwaggerOperation(
        Summary = "updates an existing registration material permit capacity",
        Description = "attempting to update the registration material permit capacity."
    )]
    public async Task<IActionResult> UpdateRegistrationMaterialPermitCapacity([FromRoute] Guid id, [FromBody] UpdateRegistrationMaterialPermitCapacityDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationMaterialPermitCapacity, id);

        _ = await _registrationMaterialService.UpdateRegistrationMaterialPermitCapacityAsync(id, request);

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
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }

    [HttpDelete("{registrationMaterialId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "delete a registration material",
        Description = "attempting to delete a material registration."
    )]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> DeleteRegistrationMaterial([FromRoute] Guid registrationMaterialId)
    {
        _logger.LogInformation(LogMessages.DeleteRegistrationMaterial, registrationMaterialId);

        try
        {
            if (await _registrationMaterialService.Delete(registrationMaterialId))
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }

	[HttpPost("UpdateIsMaterialRegistered")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResult))]
	[SwaggerOperation(
	Summary = "updates an existing registration material IsMaterialRegistered flag",
	Description = "attempting to update the registration material IsMaterialRegistered flag."
	)]
	public async Task<IActionResult> UpdateIsMaterialRegisteredAsync([FromBody] List<UpdateIsMaterialRegisteredDto> request)
	{
		_logger.LogInformation(LogMessages.UpdateIsMaterialRegistered);

		_ = await _registrationMaterialService.UpdateIsMaterialRegisteredAsync(request);

		return NoContent();
	}

    [HttpPost("{id:Guid}/contact")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationMaterialContactDto))]
    [SwaggerOperation(
        Summary = "Upserts the contact for a registration material",
        Description = "attempting to upsert the registration material contact."
    )]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpsertRegistrationMaterialContactAsync([FromRoute] Guid id, [FromBody] RegistrationMaterialContactDto request)
    {
        try
        {
            _logger.LogInformation(LogMessages.UpsertRegistrationMaterialContact, id);

            var response = await _registrationMaterialService.UpsertRegistrationMaterialContactAsync(id, request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }

    [HttpPost("{registrationMaterialId:Guid}/registrationReprocessingDetails")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationReprocessingIORequestDto))]
    [SwaggerOperation(
      Summary = "Upserts the registration reprocessing io details for a registration material",
      Description = "attempting to upsert the registration reprocessing io details."
  )]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpsertRegistrationReprocessingDetailsAsync([FromRoute] Guid registrationMaterialId, [FromBody] RegistrationReprocessingIORequestDto request)
    {
        try
        {
            _logger.LogInformation(LogMessages.UpsertRegistrationReprocessingDetails, registrationMaterialId);

            await _registrationMaterialService.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialId, request);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }

    [HttpPost("{registrationMaterialId:Guid}/materialNotReprocessingReason")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationReprocessingIORequestDto))]
    [SwaggerOperation(
      Summary = "Update the reason for not reprocessing a registration material",
      Description = "attempting to update the reason for not reprocessing a registration material."
    )]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateMaterialNotReprocessingReasonAsync([FromRoute] Guid registrationMaterialId, [FromBody] string materialNotReprocessingReason)
    {
        try
        {
            _logger.LogInformation(LogMessages.UpdateMaterialNotReprocessingReason, registrationMaterialId);

            await _registrationMaterialService.UpdateMaterialNotReprocessingReasonAsync(registrationMaterialId, materialNotReprocessingReason);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.UnExpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }
}