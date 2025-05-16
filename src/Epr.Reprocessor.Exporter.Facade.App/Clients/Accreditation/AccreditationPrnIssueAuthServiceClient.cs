namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Extensions;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Microsoft.Extensions.Options;

public class AccreditationPrnIssueAuthServiceClient : BaseHttpClient, IAccreditationPrnIssueAuthServiceClient
{
    private readonly PrnBackendServiceApiConfig config;

    public AccreditationPrnIssueAuthServiceClient(HttpClient httpClient, IOptions<PrnBackendServiceApiConfig> options)
        : base(httpClient)
    {
        config = options.Value;
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-ORGANISATION", Guid.NewGuid().ToString()); // TODO
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-USER", Guid.NewGuid().ToString()); // TODO
    }

    public async Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId)
    {
        var url = string.Format(config.Endpoints.AccreditationPrnIssueAuthGet, config.ApiVersion, accreditationId);

        return await GetAsync<List<AccreditationPrnIssueAuthDto>>(url);
    }

    public async Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> requestDtos)
    {
        var url = string.Format(config.Endpoints.AccreditationPrnIssueAuthPost, config.ApiVersion, accreditationId);

        await PostAsync<List<AccreditationPrnIssueAuthRequestDto>>(url, requestDtos);
    }
}