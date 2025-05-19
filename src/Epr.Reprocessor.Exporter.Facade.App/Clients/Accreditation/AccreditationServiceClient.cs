namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

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
}