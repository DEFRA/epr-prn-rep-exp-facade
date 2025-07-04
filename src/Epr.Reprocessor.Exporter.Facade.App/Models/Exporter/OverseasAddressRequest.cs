using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;

[ExcludeFromCodeCoverage]
public class OverseasAddressRequest
{
    public Guid? RegistrationMaterialId { get; set; }
    public List<OverseasAddress>? OverseasAddresses { get; set; }
}
