using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public record OverseasAddressDto(
    Guid ExternalId,
    string OrganisationName,
    string AddressLine1,
    string AddressLine2,
    string CityOrTown,
    string StateProvince,
    string PostCode,
    string CountryName,
    string SiteCoordinates,
    Guid CreatedBy,
    List<OverseasAddressContactDto> OverseasAddressContacts,
    List<OverseasAddressWasteCodesDto> OverseasAddressWasteCodes
);
