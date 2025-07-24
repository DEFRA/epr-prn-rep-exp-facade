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
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
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

    [HttpGet("Files/{externalId}")]
    [ProducesResponseType(typeof(List<AccreditationFileUploadDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFileUpload([FromRoute] Guid externalId)
    {
        var fileUpload = await service.GetFileUpload(externalId);
        return fileUpload != null ? Ok(fileUpload) : NotFound();
    }

    [HttpGet("{accreditationId}/Files/{fileUploadTypeId}/{fileUploadStatusId?}")]
    [ProducesResponseType(typeof(List<AccreditationFileUploadDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFileUploads([FromRoute] Guid accreditationId, [FromRoute] int fileUploadTypeId, [FromRoute] int fileUploadStatusId = 1)
    {
        var fileUploads = await service.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);
        return fileUploads != null ? Ok(fileUploads) : NotFound();
    }

    [HttpPost("{accreditationId}/Files")]
    [ProducesResponseType(typeof(AccreditationFileUploadDto), 200)]
    public async Task<IActionResult> UpsertFileUpload([FromRoute] Guid accreditationId, [FromBody] AccreditationFileUploadDto request)
    {
        var fileUpload = await service.UpsertFileUpload(accreditationId, request);
        return Ok(fileUpload);
    }

    [HttpDelete("{accreditationId}/Files/{fileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFileUpload([FromRoute] Guid accreditationId, [FromRoute] Guid fileId)
    {
        await service.DeleteFileUpload(accreditationId, fileId);

        return Ok();
    }

    [HttpGet("{organisationid:guid}/overview")]
    [ProducesResponseType(typeof(List<AccreditationOverviewDto>), 200)]
    public async Task<IActionResult> GetAccreditationOverviewByOrgId([FromRoute] Guid organisationId)
    {
        var accreditationOverviews = await service.GetAccreditationOverviewByOrgId(organisationId);

        return Ok(accreditationOverviews);
    }
}