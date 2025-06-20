namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public class AccreditationService(IAccreditationServiceClient serviceClient) : IAccreditationService
{
    public async Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        return await serviceClient.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);
    }

    public async Task<AccreditationDto> GetAccreditationById(Guid accreditationId)
    {
        return await serviceClient.GetAccreditationById(accreditationId);
    }

    public async Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto requestDto)
    {
        return await serviceClient.UpsertAccreditation(requestDto);
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        await serviceClient.ClearDownDatabase();
    }

    public async Task<List<AccreditationFileUploadDto>> GetFileUploads(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId)
    {
        List<AccreditationFileUploadDto> fileUploads = await serviceClient.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);
        return fileUploads;
    }

    public async Task<AccreditationFileUploadDto> UpsertFileUpload(Guid accreditationId, AccreditationFileUploadDto request)
    {
        var fileUpload = await serviceClient.UpsertFileUpload(accreditationId, request);
        return fileUpload;
    }

    public async Task DeleteFileUpload(Guid accreditationId, Guid fileId)
    {
        await serviceClient.DeleteFileUpload(accreditationId, fileId);
    }
}
