using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class CreateRegistrationMaterialRequestDto
{
    public Guid RegistrationId { get; set; }

    public string Material { get; set; }
}
