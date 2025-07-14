using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

public interface IOverseasAccreditationSiteServiceClient
{
    Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request);
}
