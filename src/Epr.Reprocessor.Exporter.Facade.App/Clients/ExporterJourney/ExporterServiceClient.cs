using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Extensions;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

[ExcludeFromCodeCoverage]
public class ExporterServiceClient : BaseHttpClient, IExporterServiceClient
{
    public ExporterServiceClient(HttpClient httpClient, IOptions<PrnBackendServiceApiConfig> options)
    : base(httpClient)
    {
        httpClient.DefaultRequestHeaders.AddIfNotExists("X-EPR-USER", Guid.NewGuid().ToString());
    }

    public async Task<TOut> SendGetRequest<TOut>(string uri)
    {
        return await GetAsync<TOut>(uri);
    }

    public async Task<Guid> SendPostRequest<TBody>(string uri, TBody body)
    {
        return await this.PostAsync<TBody, Guid>(uri, body);
    }

    public async Task SendPutRequest<TBody>(string uri, TBody body)
    {
        await this.PutAsync<TBody>(uri, body);
    }
}