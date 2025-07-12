using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

public class OverseasAccreditationSiteService(IOverseasAccreditationSiteServiceClient serviceClient) : IOverseasAccreditationSiteService
{
    public async Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId)
    {
        return await serviceClient.GetAllByAccreditationId(accreditationId);
    }

    public async Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request)
    {
        await serviceClient.PostByAccreditationId(accreditationId, request);
    }
}
