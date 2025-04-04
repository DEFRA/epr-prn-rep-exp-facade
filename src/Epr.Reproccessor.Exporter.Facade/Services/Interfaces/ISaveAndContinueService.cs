using Epr.Reproccessor.Exporter.Facade.Api.Models;

namespace Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces
{
    public interface ISaveAndContinueService
    {
        Task<HttpResponseMessage> AddAsync(SaveAndContinueRequest request);
        Task<SaveAndContinueResponse?> GetLatestAsync(int registrationId, string controller, string area);
    }
}
