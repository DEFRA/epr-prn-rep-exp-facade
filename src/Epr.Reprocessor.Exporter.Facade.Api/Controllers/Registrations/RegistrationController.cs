using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

[Route("api/v{version:apiVersion}/Registrations")]
[ApiVersion("1.0")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
    {
        ArgumentNullException.ThrowIfNull(registrationService);
        ArgumentNullException.ThrowIfNull(logger);

        _registrationService = registrationService;
        _logger = logger;
    }

    [HttpGet("{applicationTypeId:int}/organisations/{organisationId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration isn not found.", typeof(ProblemDetails))]
    [SwaggerOperation(
        Summary = "gets an existing registration by the organisation ID.",
        Description = "attempting to get an existing registration using the organisation ID."
    )]
    public async Task<IActionResult> GetRegistrationByOrganisation([FromRoute] int applicationTypeId, [FromRoute] Guid organisationId)
    {
        _logger.LogInformation(LogMessages.GetRegistrationByOrganisation, applicationTypeId, organisationId);

        var registration = await _registrationService.GetRegistrationByOrganisationAsync(applicationTypeId, organisationId);

        if (registration is null)
        {
            return NotFound();
        }

        return Ok(registration);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [SwaggerOperation(
            Summary = "create an application registration",
            Description = "attempting to create an application registration."
        )]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto request)
    {
        _logger.LogInformation(LogMessages.CreateRegistration);

        if (request.ApplicationTypeId is 0 || request.OrganisationId == Guid.Empty)
        {
            return BadRequest($"The {nameof(request.ApplicationTypeId)} and {nameof(request.OrganisationId)} must have a valid non 0/empty value.");
        }

        var registrationId = await _registrationService.CreateRegistrationAsync(request);

        return new CreatedResult(string.Empty, registrationId);
    }

    [HttpPost("{registrationId:guid}/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [SwaggerOperation(
        Summary = "update an application registration",
        Description = "attempting to update an application registration."
    )]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid registrationId, [FromBody] UpdateRegistrationDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationTaskStatus);

        await _registrationService.UpdateAsync(registrationId, request);

        return NoContent();
    }

    [HttpPost("{registrationId:guid}/TaskStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [SwaggerOperation(
            Summary = "update the task status of an application registration",
            Description = "attempting to update the task status of an application registration."
        )]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromRoute] Guid registrationId, [FromBody] UpdateRegistrationTaskStatusDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationTaskStatus);

        await _registrationService.UpdateRegistrationTaskStatusAsync(registrationId, request);

        return NoContent();
    }

    [HttpPost("{registrationId:guid}/SiteAddress")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [SwaggerOperation(
            Summary = "update the site address and contact details of an application registration",
            Description = "attempting to update the site address and contact details of an application registration."
        )]
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] Guid registrationId, [FromBody] UpdateRegistrationSiteAddressDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);

        await _registrationService.UpdateSiteAddressAsync(registrationId, request);

        return NoContent();
    }

    [HttpGet("{registrationId:int}/RegistrationTaskStatus")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationTaskDto>))]
    [SwaggerOperation(
        Summary = "get the task statuses of a registration",
        Description = "retrieving a list of task statuses for a registration."
    )]
    public async Task<IActionResult> RegistrationTaskStatus([FromRoute] Guid registrationId )
    {
        _logger.LogInformation(LogMessages.GetRegistrationOverview, registrationId);

        var overview = await _registrationService.GetRegistrationOverviewAsync(registrationId);

        if (overview == null)
        {
            return NoContent();
        }

        return Ok(overview.Tasks);
    }
}