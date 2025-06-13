using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
	public class WasteCarrierBrokerDealerRefService : IWasteCarrierBrokerDealerRefService
	{
		private readonly PrnBackendServiceApiConfig _config;
		private readonly IExporterServiceClient _apiClient;
		private int _apiVersion;
		private string _baseGetUrl;
		private string _basePostUrl;
		private string _basePutUrl;

		public WasteCarrierBrokerDealerRefService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
		{
			_config = options.Value;
			_apiClient = apiClient;
			_apiVersion = _config.ApiVersion;

			_baseGetUrl = _config.ExportEndpoints.WasteCarrierBrokerDealerRefGet;
			_basePostUrl = _config.ExportEndpoints.WasteCarrierBrokerDealerRefPost;
			_basePutUrl = _config.ExportEndpoints.WasteCarrierBrokerDealerRefPut;
		}

		public async Task<Guid> Create(int registrationId, WasteCarrierBrokerDealerRefDto value)
		{
			var uri = string.Format(_basePostUrl, _apiVersion, registrationId);
			var result = await _apiClient.SendPostRequest<WasteCarrierBrokerDealerRefDto>(uri, value);
			return result;
		}

		public async Task<WasteCarrierBrokerDealerRefDto> Get(int registrationId)
		{
			var uri = string.Format(_baseGetUrl, _apiVersion, registrationId);
			var dto = await _apiClient.SendGetRequest<WasteCarrierBrokerDealerRefDto>(uri);
			return dto;
		}

		public async Task<bool> Update(int registrationId, WasteCarrierBrokerDealerRefDto value)
		{
			var uri = string.Format(_basePutUrl, _apiVersion, registrationId, value.Id);
			var result = await _apiClient.SendPutRequest<WasteCarrierBrokerDealerRefDto>(uri, value);
			return true;
		}
	}
}
