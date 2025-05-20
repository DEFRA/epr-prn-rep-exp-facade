using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class CreateRegistrationDto
{
    public int ApplicationTypeId { get; set; }
    public int OrganisationId { get; set; }
}