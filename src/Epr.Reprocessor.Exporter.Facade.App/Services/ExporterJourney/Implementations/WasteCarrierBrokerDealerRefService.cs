using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations;

public class WasteCarrierBrokerDealerRefService : IWasteCarrierBrokerDealerRefService
{
    private readonly IExporterServiceClient _apiClient;
    private readonly int _apiVersion;
    private readonly string _baseGetUrl;
    private readonly string _basePostUrl;
    private readonly string _basePutUrl;

    public WasteCarrierBrokerDealerRefService(IExporterServiceClient apiClient,
        IOptions<PrnBackendServiceApiConfig> options)
    {
        var config = options.Value;

        _apiClient = apiClient;
        _apiVersion = config.ApiVersion;

        _baseGetUrl = config.ExportEndpoints.WasteCarrierBrokerDealerRefGet;
        _basePostUrl = config.ExportEndpoints.WasteCarrierBrokerDealerRefPost;
        _basePutUrl = config.ExportEndpoints.WasteCarrierBrokerDealerRefPut;
    }

    public async Task<Guid> Create(Guid registrationId, WasteCarrierBrokerDealerRefDto value)
    {
        var uri = string.Format(_basePostUrl, _apiVersion, registrationId);
        var result = await _apiClient.SendPostRequest<WasteCarrierBrokerDealerRefDto>(uri, value);
        return result;
    }

    public async Task<WasteCarrierBrokerDealerRefDto> Get(Guid registrationId)
    {
        var uri = string.Format(_baseGetUrl, _apiVersion, registrationId);
        var dto = await _apiClient.SendGetRequest<WasteCarrierBrokerDealerRefDto>(uri);
        return dto;
    }

    public async Task<bool> Update(Guid registrationId, WasteCarrierBrokerDealerRefDto value)
    {
        var uri = string.Format(_basePutUrl, _apiVersion, registrationId);
        await _apiClient.SendPutRequest<WasteCarrierBrokerDealerRefDto>(uri, value);
        return true;
    }
}