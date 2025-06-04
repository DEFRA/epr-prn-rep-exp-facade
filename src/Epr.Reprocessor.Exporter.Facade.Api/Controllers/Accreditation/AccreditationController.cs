namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;

using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.Api.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Accreditation")]
[ApiController]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class AccreditationController(IAccreditationService service) : ControllerBase
{
    [HttpGet("{organisationId}/{materialId}/{applicationTypeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid organisationId,
        [FromRoute] int materialId,
        [FromRoute] int applicationTypeId)
    {
        var accreditation = await service.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);

        return Ok(accreditation);
    }

    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(AccreditationDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid accreditationId)
    {
        var accreditation = await service.GetAccreditationById(accreditationId);
        return accreditation == null ? NotFound() : Ok(accreditation);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccreditationDto), 200)]
    public async Task<IActionResult> Post([FromBody] AccreditationRequestDto request)
    {
        var accreditation = await service.UpsertAccreditation(request);
        return Ok(accreditation);
    }

    [HttpPost("clear-down-database")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ExcludeFromCodeCoverage]
    public async Task<IActionResult> ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        await service.ClearDownDatabase();

        return Ok();
    }
}