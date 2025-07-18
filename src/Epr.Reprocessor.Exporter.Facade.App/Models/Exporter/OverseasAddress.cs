using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;

[ExcludeFromCodeCoverage]
public class OverseasAddress
{
    [MaxLength(100)]
    public required string AddressLine1 { get; set; }
    [MaxLength(100)]
    public required string AddressLine2 { get; set; }
    [MaxLength(70)]
    public required string CityOrTown { get; set; }
    public required string CountryName { get; set; }
    public Guid Id { get; set; }
    public required string OrganisationName { get; set; }
    [MaxLength(20)]
    public required string PostCode { get; set; }
    [MaxLength(100)]
    public required string SiteCoordinates { get; set; }
    [MaxLength(70)]
    public required string StateProvince { get; set; }
    public List<OverseasAddressContact> OverseasAddressContacts { get; set; } = new();
    public List<OverseasAddressWasteCodes> OverseasAddressWasteCodes { get; set; } = new();
}
