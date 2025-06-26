using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
    public class OtherPermitsService : IOtherPermitsService
	{
		private readonly IExporterServiceClient _apiClient;
		private readonly int _apiVersion;
		private readonly string _baseGetUrl;
		private readonly string _basePostUrl;
		private readonly string _basePutUrl;

		public OtherPermitsService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
		{
			var config = options.Value;
			_apiClient = apiClient;
			_apiVersion = config.ApiVersion;

			_baseGetUrl = config.ExportEndpoints.OtherPermitsGet;
			_basePostUrl = config.ExportEndpoints.OtherPermitsPost;
			_basePutUrl = config.ExportEndpoints.OtherPermitsPut;
		}

		public async Task<CarrierBrokerDealerPermitsDto> Get(Guid registrationId)
		{
			var uri = string.Format(_baseGetUrl, _apiVersion, registrationId);
			var dto = await _apiClient.SendGetRequest<CarrierBrokerDealerPermitsDto>(uri);
			return dto;
		}

		public async virtual Task<Guid> Create(Guid registrationId, CarrierBrokerDealerPermitsDto value)
		{
			var uri = string.Format(_basePostUrl, _apiVersion, registrationId);
			var result = await _apiClient.SendPostRequest<CarrierBrokerDealerPermitsDto>(uri, value);
			return result;
		}

		public async virtual Task<bool> Update(Guid registrationId, CarrierBrokerDealerPermitsDto value)
		{
			var uri = string.Format(_basePutUrl, _apiVersion, registrationId, value.CarrierBrokerDealerPermitId);
			await _apiClient.SendPutRequest<CarrierBrokerDealerPermitsDto>(uri, value);
			return true;
		}
	}
}
