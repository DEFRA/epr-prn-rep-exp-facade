using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;

[ExcludeFromCodeCoverage]
public class OverseasAddressWasteCodes
{
    public Guid Id { get; set; }
    public required string CodeName { get; set; }
}
