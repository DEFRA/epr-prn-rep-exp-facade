using System.Diagnostics.CodeAnalysis;
using System.Net;
using Azure.Core;
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

    [HttpGet("{applicationTypeId:int}/organisations/{organisationId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "If an existing registration isn not found.", typeof(ProblemDetails))]
    [SwaggerOperation(
        Summary = "gets an existing registration by the organisation ID.",
        Description = "attempting to get an existing registration using the organisation ID."
    )]
    [ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
    public async Task<IActionResult> GetRegistrationByOrganisation([FromRoute] int applicationTypeId, [FromRoute] int organisationId)
    {
        _logger.LogInformation(string.Format(LogMessages.GetRegistrationByOrganisation, applicationTypeId, organisationId));

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
    [ExcludeFromCodeCoverage(Justification = "TODO: Unit tests to be added as part of create registration user story")]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto request)
    {
        _logger.LogInformation(LogMessages.CreateRegistration);

        var registrationId = await _registrationService.CreateRegistrationAsync(request);

        return new CreatedResult(string.Empty, registrationId);
    }

    [HttpPost("{registrationId:int}/TaskStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [SwaggerOperation(
            Summary = "update the task status of an application registration",
            Description = "attempting to update the task status of an application registration."
        )]
    public async Task<IActionResult> UpdateRegistrationTaskStatus([FromRoute] int registrationId, [FromBody] UpdateRegistrationTaskStatusDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationTaskStatus);

        await _registrationService.UpdateRegistrationTaskStatusAsync(registrationId, request);

        return NoContent();
    }

    [HttpPost("{registrationId:int}/SiteAddress")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [SwaggerOperation(
            Summary = "update the site address and contact details of an application registration",
            Description = "attempting to update the site address and contact details of an application registration."
        )]
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] int registrationId, [FromBody] UpdateRegistrationSiteAddressDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);

        await _registrationService.UpdateSiteAddressAsync(registrationId, request);

        return NoContent();
    }
}
