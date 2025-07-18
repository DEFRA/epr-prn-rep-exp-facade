using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using System.Collections.Generic;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

public interface IAccreditationService
{
    Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task<AccreditationDto> GetAccreditationById(Guid accreditationId);

    Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto accreditationDto);

    Task ClearDownDatabase();

    Task<AccreditationFileUploadDto> GetFileUpload(Guid externalId);

    Task<List<AccreditationFileUploadDto>> GetFileUploads(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId);

    Task<AccreditationFileUploadDto> UpsertFileUpload(Guid accreditationId, AccreditationFileUploadDto request);

    Task DeleteFileUpload(Guid accreditationId, Guid fileId);

    Task<List<AccreditationOverviewDto>> GetAccreditationOverviewByOrgId(Guid organisationId);
}
