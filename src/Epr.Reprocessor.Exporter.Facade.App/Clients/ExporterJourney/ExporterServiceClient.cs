using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

[ExcludeFromCodeCoverage]
public class ExporterServiceClient : BaseHttpClient, IExporterServiceClient
{
    private readonly PrnBackendServiceApiConfig _config;
	private readonly ILogger<ExporterServiceClient> _logger;

	public ExporterServiceClient(HttpClient httpClient, IOptions<PrnBackendServiceApiConfig> options, ILogger<ExporterServiceClient> logger)
	: base(httpClient)
	{
		_config = options.Value;
		_logger = logger;
		httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-USER", Guid.NewGuid().ToString()); // TODO: assign user Id
	}

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