using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Defines a lookup dto for the exemption references of a material.
/// </summary>
[ExcludeFromCodeCoverage]
public record ExemptionReferencesLookupDto
{
    public string ReferenceNumber { get; set; } = null!;
}