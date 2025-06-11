using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Services
{
    public class BaseReprocessorExporterService<TDto> : IBaseReprocessorExporterService<TDto>
    {
        protected readonly PrnBackendServiceApiConfig Config;
        protected string BaseGetUrl;
        protected string BasePostUrl;

        private readonly IExporterServiceClient apiClient;

        public BaseReprocessorExporterService(IExporterServiceClient apiClient, IOptions<PrnBackendServiceApiConfig> options)
        {
            Config = options.Value;
            this.apiClient = apiClient;
        }

        public async virtual Task<IEnumerable<TDto>> Get()
        {
            var dtos = await apiClient.SendGetRequest<IEnumerable<TDto>>(BaseGetUrl);
            return dtos;
        }

        public async virtual Task<TDto> Get(int id)
        {
            var dto = await apiClient.SendGetRequest<TDto>($"{BaseGetUrl}/{id}");
            return dto;
        }

        public async virtual Task<int> Create(TDto value)
        {
            var result = await apiClient.SendPostRequest<TDto, int>(BasePostUrl, value);
            return result;
        }

        public async virtual Task Update(int id, TDto value)
        {
            throw new NotImplementedException();
        }

        public async virtual Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
