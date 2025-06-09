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

	public async Task<TOut> SendGetRequest<TOut>(string uri)
	{
		return await GetAsync<TOut>(uri);
	}

	public async Task<TOut> SendPostRequest<TBody, TOut>(string uri, TBody body)
	{
		return await this.PostAsync<TBody, TOut>(uri, body);
	}
}