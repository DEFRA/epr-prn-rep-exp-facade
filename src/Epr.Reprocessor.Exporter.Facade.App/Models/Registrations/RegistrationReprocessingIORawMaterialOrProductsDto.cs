using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class RegistrationReprocessingIORawMaterialOrProductsDto
{
    public int RegistrationReprocessingIOId { get; set; }

    public string RawMaterialOrProductName { get; set; }

    public decimal TonneValue { get; set; }

    public bool IsInput { get; set; }
}