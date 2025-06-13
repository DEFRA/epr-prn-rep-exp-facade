using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
    public class OtherPermitsService : IOtherPermitsService
	{
		private readonly PrnBackendServiceApiConfig _config;
		private readonly IExporterServiceClient _apiClient;
		private int _apiVersion;
		private string _baseGetUrl;
		private string _basePostUrl;
		private string _basePutUrl;

		public OtherPermitsService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
		{
			_config = options.Value;
			_apiClient = apiClient;
			_apiVersion = _config.ApiVersion;

			_baseGetUrl = _config.ExportEndpoints.OtherPermitsGet;
			_basePostUrl = _config.ExportEndpoints.OtherPermitsPost;
			_basePutUrl = _config.ExportEndpoints.OtherPermitsPut;
		}

		public async Task<OtherPermitsDto> Get(int id)
		{
			var uri = string.Format(_baseGetUrl, _apiVersion, id);
			var dto = await _apiClient.SendGetRequest<OtherPermitsDto>(uri);
			return dto;
		}

		public async virtual Task<Guid> Create(int registrationId, OtherPermitsDto value)
		{
			var uri = string.Format(_basePostUrl, _apiVersion, registrationId);
			var result = await _apiClient.SendPostRequest<OtherPermitsDto>(uri, value);
			return result;
		}

		public async virtual Task<bool> Update(int registrationId, OtherPermitsDto value)
		{
			var uri = string.Format(_basePutUrl, _apiVersion, registrationId, value.Id);
			var result = await _apiClient.SendPutRequest<OtherPermitsDto>(uri, value);
			return true;
		}
	}
}
