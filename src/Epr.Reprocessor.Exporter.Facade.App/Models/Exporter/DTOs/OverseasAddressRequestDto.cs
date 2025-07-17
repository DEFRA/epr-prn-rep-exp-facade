using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public record OverseasAddressRequestDto(
    Guid? RegistrationMaterialId,
    List<OverseasAddressDto>? OverseasAddresses
)
{
    public static OverseasAddressRequestDto MapOverseasAddressRequestToDto(OverseasAddressRequest? request, Guid createdBy)
    {
        if (request == null)
        {
            return new OverseasAddressRequestDto(null, null);
        }

        var overseasAddressesDto = request.OverseasAddresses?.Select(address => new OverseasAddressDto(
            address.Id,
            address.OrganisationName,
            address.AddressLine1,
            address.AddressLine2,
            address.CityOrTown,
            address.StateProvince,
            address.PostCode,
            address.CountryName,
            address.SiteCoordinates,
            createdBy,
            address.OverseasAddressContacts.Select(contact => new OverseasAddressContactDto(
                contact.FullName,
                contact.Email,
                contact.PhoneNumber,
                createdBy)).ToList(),
            address.OverseasAddressWasteCodes.Select(wasteCode => new OverseasAddressWasteCodesDto(
                wasteCode.ExternalId,
                wasteCode.CodeName)).ToList()
        )).ToList();

        return new OverseasAddressRequestDto(
            request.RegistrationMaterialId,
            overseasAddressesDto
        );
    }
}

