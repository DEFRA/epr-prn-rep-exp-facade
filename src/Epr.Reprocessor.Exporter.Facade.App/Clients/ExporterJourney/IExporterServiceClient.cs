namespace Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;

public interface IExporterServiceClient
{
	Task<TOut> SendGetRequest<TOut>(string uri);

	Task<TOut> SendPostRequest<TBody, TOut>(string uri, TBody body);

}
