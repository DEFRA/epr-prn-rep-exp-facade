using Epr.Reprocessor.Exporter.Facade.Api.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/OverseasAccreditationSite")]
[FeatureGate(FeatureFlags.EnableAccreditation)]
public class OverseasAccreditationSiteController(IOverseasAccreditationSiteService service) : ControllerBase
{
    [HttpGet("{accreditationId}")]
    [ProducesResponseType(typeof(List<OverseasAccreditationSiteDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByAccreditationId([FromRoute] Guid accreditationId)
    {
        var overseasAccreditationSites = await service.GetAllByAccreditationId(accreditationId);

        if (overseasAccreditationSites == null)
        {
            return NotFound();
        }

        return Ok(overseasAccreditationSites);
    }

    [HttpPost("{accreditationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Post([FromRoute] Guid accreditationId, [FromBody] OverseasAccreditationSiteDto request)
    {
        await service.PostByAccreditationId(accreditationId, request);

        return NoContent();
    }
}
