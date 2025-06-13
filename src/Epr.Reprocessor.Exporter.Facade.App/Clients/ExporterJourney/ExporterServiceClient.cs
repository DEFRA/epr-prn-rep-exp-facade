using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

[ExcludeFromCodeCoverage]
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

	public async Task<Guid> SendPostRequest<TBody>(string uri, TBody body)
	{
		return await this.PostAsync<TBody, Guid>(uri, body);
	}

	public async Task<TBody> SendPutRequest<TBody>(string uri, TBody body)
	{
		return await this.PutAsync<TBody, TBody>(uri, body);
	}
}