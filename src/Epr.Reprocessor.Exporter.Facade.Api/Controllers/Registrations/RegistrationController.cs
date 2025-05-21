using System.Diagnostics.CodeAnalysis;
using System.Net;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [SwaggerOperation(
            Summary = "create an application registration",
            Description = "attempting to create an application registration."
        )]
    [SwaggerResponse(StatusCodes.Status201Created, $"Returns Registration Id", typeof(int))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    [ExcludeFromCodeCoverage(Justification = "TODO: Unit tests to be added as part of create registration user story")]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto request)
    {
        _logger.LogInformation(LogMessages.CreateRegistration);

        var registrationId = await _registrationService.CreateRegistrationAsync(request);

        return new CreatedResult(string.Empty, registrationId);
    }

    [HttpPost("{registrationId:int}/TaskStatus")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the task status of an application registration",
            Description = "attempting to update the task status of an application registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
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
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateSiteAddress([FromRoute] int registrationId, [FromBody] UpdateRegistrationSiteAddressDto request)
    {
        _logger.LogInformation(LogMessages.UpdateRegistrationSiteAddress);

        await _registrationService.UpdateSiteAddressAsync(registrationId, request);

        return NoContent();
    }
}
