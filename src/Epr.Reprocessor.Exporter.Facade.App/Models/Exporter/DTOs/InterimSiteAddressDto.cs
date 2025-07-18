using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public class InterimSiteAddressDto : OverseasAddressBaseDto
{
    public List<OverseasAddressContactDto> InterimAddressContact { get; set; } = new();
}