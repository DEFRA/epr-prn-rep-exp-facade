namespace Epr.Reprocessor.Exporter.Facade.App.Services
{
    public interface IBaseReprocessorExporterService<TDto>
    {
        Task<IEnumerable<TDto>> Get();

        Task<TDto> Get(int id);

        Task<int> Create(TDto value);

        Task Update(int id, TDto value);

        Task Delete(int id);
    }
}
