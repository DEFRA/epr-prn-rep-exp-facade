using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations
{
	public class BaseExporterService<TDtoIn, TDtoOut> : IExporterService<TDtoIn, TDtoOut>
	{
		protected readonly PrnBackendServiceApiConfig Config;
		protected string BaseGetUrl;
		protected string BasePostUrl;

		private readonly IExporterServiceClient apiClient;

        public BaseExporterService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
        {
			Config = options.Value;
			this.apiClient = apiClient;
		}

		public async virtual Task<IEnumerable<TDtoOut>> Get()
		{
			var dtos = await apiClient.SendGetRequest<IEnumerable<TDtoOut>>(BaseGetUrl);
			return dtos;
		}

		public async virtual Task<TDtoOut> Get(int id)
		{
			var dto = await apiClient.SendGetRequest<TDtoOut>($"{BaseGetUrl}/{id}");
			return dto;
		}

		public async virtual Task<int> Create(TDtoIn value)
		{
			var result = await apiClient.SendPostRequest<TDtoIn, int>(BasePostUrl, value);
			return result;
		}

		public async virtual Task Update(int id, TDtoIn value)
		{
			throw new NotImplementedException();
		}

		public async virtual Task Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
