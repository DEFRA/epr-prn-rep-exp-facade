using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public class OverseasMaterialReprocessingSiteDto
{
    public Guid Id { get; init; }
    public Guid OverseasAddressId { get; init; }
    public required OverseasAddressBaseDto OverseasAddress { get; init; }
    public List<InterimSiteAddressDto>? InterimSiteAddresses { get; set; } = new();
}