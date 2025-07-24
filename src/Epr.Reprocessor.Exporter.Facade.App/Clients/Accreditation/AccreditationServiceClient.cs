namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Extensions;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Microsoft.Extensions.Options;

public class AccreditationServiceClient : BaseHttpClient, IAccreditationServiceClient
{
    private readonly PrnBackendServiceApiConfig config;

    public AccreditationServiceClient(HttpClient httpClient, IOptions<PrnBackendServiceApiConfig> options)
        :base(httpClient)
    {
        config = options.Value;
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-ORGANISATION", Guid.NewGuid().ToString()); // TODO
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-USER", Guid.NewGuid().ToString()); // TODO
    }

    public async Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        var url = string.Format(config.Endpoints.AccreditationGetOrCreate, config.ApiVersion, organisationId, materialId, applicationTypeId);

        return await GetAsync<Guid>(url);
    }

    public async Task<AccreditationDto> GetAccreditationById(Guid accreditationId)
    {
        var url = string.Format(config.Endpoints.AccreditationGet, config.ApiVersion, accreditationId);

        return await GetAsync<AccreditationDto>(url);
    }    

    public async Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto requestDto)
    {
        var url = string.Format(config.Endpoints.AccreditationPost, config.ApiVersion);

        return await PostAsync<AccreditationRequestDto, AccreditationDto>(url, requestDto);
    }

    [ExcludeFromCodeCoverage]
    public async Task ClearDownDatabase()
    {
        // Temporary: Aid to QA whilst Accreditation uses in-memory database.
        var url = string.Format("api/v{0}/accreditation/clear-down-database", config.ApiVersion);
        
        await PostAsync<Object>(url, null);
    }

    public async Task<AccreditationFileUploadDto> GetFileUpload(Guid externalId)
    {
        var url = string.Format(config.Endpoints.AccreditationFileUploadGet, config.ApiVersion, externalId);
        return await GetAsync<AccreditationFileUploadDto>(url);
    }

    public async Task<List<AccreditationFileUploadDto>> GetFileUploads(Guid accreditationId, int fileUploadTypeId, int fileUploadStatusId)
    {
        var url = string.Format(config.Endpoints.AccreditationFileUploadsGet, config.ApiVersion, accreditationId, fileUploadTypeId, fileUploadStatusId);

        return await GetAsync<List<AccreditationFileUploadDto>>(url);
    }

    public async Task<AccreditationFileUploadDto> UpsertFileUpload(Guid accreditationId, AccreditationFileUploadDto request)
    {
        var url = string.Format(config.Endpoints.AccreditationFileUploadPost, config.ApiVersion, accreditationId);

        return await PostAsync<AccreditationFileUploadDto, AccreditationFileUploadDto>(url, request);
    }

    public async Task DeleteFileUpload(Guid accreditationId, Guid fileId)
    {
        var url = string.Format(config.Endpoints.AccreditationFileUploadDelete, config.ApiVersion, accreditationId, fileId);

        await DeleteAsync(url);
    }

    public async Task<List<AccreditationOverviewDto>> GetAccreditationOverviewByOrgId(Guid organisationId)
    {
        var url = string.Format(config.Endpoints.AccreditationOverViewByOrgId, config.ApiVersion, organisationId);

        return await GetAsync<List<AccreditationOverviewDto>>(url);
    }
}