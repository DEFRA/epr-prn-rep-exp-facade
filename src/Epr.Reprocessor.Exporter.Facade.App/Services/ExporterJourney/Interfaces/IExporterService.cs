namespace Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces
{
	public interface IExporterService<TDtoIn, TDtoOut>
	{
		Task<IEnumerable<TDtoOut>> Get();

		Task<TDtoOut> Get(int id);

		Task<int> Create(TDtoIn value);

		Task Update(int id, TDtoIn value);

		Task Delete(int id);
	}
}
