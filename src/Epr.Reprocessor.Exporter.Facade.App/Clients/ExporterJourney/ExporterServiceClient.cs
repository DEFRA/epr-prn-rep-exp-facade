using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

public class ExporterServiceClient(
	HttpClient httpClient, 
	IOptions<PrnBackendServiceApiConfig> options, 
	ILogger<ExporterServiceClient> logger) : BaseHttpClient(httpClient), IExporterServiceClient
{
    private readonly PrnBackendServiceApiConfig _config = options.Value;
	private readonly ILogger<ExporterServiceClient> _logger = logger;

    public async Task<TOut> SendGetRequest<TOut>(string uri)
	{
		return await GetAsync<TOut>(uri);
	}

	public async Task<int> SendPostRequest<TBody>(string uri, TBody body)
	{
		return await this.PostAsync<TBody, int>(uri, body);
	}

	public async Task<bool> SendPutRequest<TBody>(string uri, TBody body)
	{
		return await this.PutAsync<TBody, bool>(uri, body);
	}
}