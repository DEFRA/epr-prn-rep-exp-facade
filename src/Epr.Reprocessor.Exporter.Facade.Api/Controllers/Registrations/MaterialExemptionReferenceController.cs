using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

/// <summary>
/// Controller for managing material exemption references in the reprocessor/exporter journey.
/// </summary>
[Route("api/v{version:apiVersion}/MaterialExemptionReferences")]
[ApiVersion("1.0")]
[ApiController]
public class MaterialExemptionReferenceController : ControllerBase
{
    private readonly IMaterialExemptionReferenceService _materialExemptionReferenceService;
    private readonly ILogger<MaterialExemptionReferenceController> _logger;

    public MaterialExemptionReferenceController(
        IMaterialExemptionReferenceService materialExemptionReferenceService,
        ILogger<MaterialExemptionReferenceController> logger)
    {
        _materialExemptionReferenceService = materialExemptionReferenceService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
    [SwaggerOperation(
            Summary = "creates an material exemption references",
            Description = "attempting to create material exemption references"
        )]
    public async Task<IActionResult> CreateMaterialExemptionReferenceAsync([FromBody] CreateMaterialExemptionReferenceDto dto)
    {
        if (dto == null)
        {
            _logger.LogWarning(LogMessages.InvalidRequest);
            return BadRequest(LogMessages.InvalidRequest);
        }

        _logger.LogInformation(LogMessages.CreateRegistration);

        var result = await _materialExemptionReferenceService.CreateMaterialExemptionReferencesAsync(dto);

        if (result)
        {
            _logger.LogInformation(LogMessages.MaterialExemptionReferencesCreated);
            return Ok();
        }

        return BadRequest(LogMessages.MaterialExemptionReferenceNotCreated);
    }
}
