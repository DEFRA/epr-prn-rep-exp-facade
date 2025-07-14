using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

public interface IOverseasAccreditationSiteService
{
    Task<List<OverseasAccreditationSiteDto>?> GetAllByAccreditationId(Guid accreditationId);

    Task PostByAccreditationId(Guid accreditationId, OverseasAccreditationSiteDto request);
}
