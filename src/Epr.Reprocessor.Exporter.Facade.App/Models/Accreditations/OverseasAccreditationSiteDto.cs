using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

[ExcludeFromCodeCoverage]
public class OverseasAccreditationSiteDto
{
    public Guid ExternalId { get; set; }

    public string OrganisationName { get; set; }

    public int MeetConditionsOfExportId { get; set; }

    public int SiteCheckStatusId { get; set; }
}
