namespace Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class AccreditationPrnIssueAuthDto
{
    public Guid ExternalId { get; set; }
    public Guid AccreditationExternalId { get; set; }
    public int PersonId { get; set; }
}
