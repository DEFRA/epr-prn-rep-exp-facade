using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;

[ExcludeFromCodeCoverage]
public record OverseasAddressWasteCodesDto(
    Guid ExternalId,
    string CodeName
);