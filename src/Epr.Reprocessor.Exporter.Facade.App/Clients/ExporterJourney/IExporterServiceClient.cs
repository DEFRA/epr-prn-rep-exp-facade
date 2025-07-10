namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

public interface IExporterServiceClient
{
	Task<TOut> SendGetRequest<TOut>(string uri);

	Task<Guid> SendPostRequest<TBody>(string uri, TBody body);

	Task SendPutRequest<TBody>(string uri, TBody body);
}
