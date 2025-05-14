using System.Net;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

[Route("api/v1")]
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

    [HttpPost(ApiUrls.UpdateSiteAddressAndContactDetails)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
            Summary = "update the site address and contact details of an application registration",
            Description = "attempting to update the site address and contact details of an application registration."
        )]
    [SwaggerResponse(StatusCodes.Status204NoContent, $"Returns No Content", typeof(NoContentResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "If the request is invalid or a validation error occurs.", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "If an unexpected error occurs.", typeof(ContentResult))]
    public async Task<IActionResult> UpdateSiteAddressAndContactDetails([FromBody] UpdateSiteAddressAndContactDetailsDto request)
    {
        _logger.LogInformation(LogMessages.UpdateSiteAddressAndContactDetails);

        await _registrationService.UpdateSiteAddressAndContactDetails(request);

        return NoContent();
    }
}
