using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public record OverseasAddressContactDto(
    string FullName,
    string Email,
    string PhoneNumber,
    Guid CreatedBy
);
