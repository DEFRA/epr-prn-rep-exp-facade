using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
    public class AddressForServiceOfNoticesService : IAddressForServiceOfNoticesService
	{
		private readonly IExporterServiceClient _apiClient;
		private readonly int _apiVersion;
		private readonly string _baseGetUrl;
		private readonly string _basePutUrl;

		public AddressForServiceOfNoticesService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
		{
			var config = options.Value;

			_apiClient = apiClient;
			_apiVersion = config.ApiVersion;

			_baseGetUrl = config.ExportEndpoints.AddressForServiceOfNoticesGet;
			_basePutUrl = config.ExportEndpoints.AddressForServiceOfNoticesPut;
		}

		public async Task<AddressForServiceOfNoticesDto> Get(Guid registrationId)
		{
			var uri = string.Format(_baseGetUrl, _apiVersion, registrationId);
			var dto = await _apiClient.SendGetRequest<AddressForServiceOfNoticesDto>(uri);
			return dto;
		}

		public async Task<bool> Update(Guid registrationId, AddressForServiceOfNoticesDto value)
		{
			var uri = string.Format(_basePutUrl, _apiVersion, registrationId);
			await _apiClient.SendPutRequest<AddressForServiceOfNoticesDto>(uri, value);
			return true;
		}
	}
}
