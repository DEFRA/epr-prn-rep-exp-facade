namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;

using Epr.Reprocessor.Exporter.Facade.Api.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/AccreditationPRNIssueAuth")]
[ApiController]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class AccreditationPrnIssueAuthController(IAccreditationPrnIssueAuthService service) : ControllerBase
{
    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(AccreditationPrnIssueAuthDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAccreditationId([FromRoute] Guid accreditationId)
    {
        List<AccreditationPrnIssueAuthDto> accreditationPrnIssueAuths = await service.GetByAccreditationId(accreditationId);

        if (accreditationPrnIssueAuths == null)
        {
            return NotFound();
        }

        return Ok(accreditationPrnIssueAuths);
    }

    [HttpPost("{accreditationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromRoute] Guid accreditationId, [FromBody] List<AccreditationPrnIssueAuthRequestDto> request)
    {
        await service.ReplaceAllByAccreditationId(accreditationId, request);

        return NoContent();
    }
}