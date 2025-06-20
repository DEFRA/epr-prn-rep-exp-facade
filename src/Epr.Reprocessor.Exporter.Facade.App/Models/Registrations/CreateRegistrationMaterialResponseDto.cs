using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents details of a created material.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreateRegistrationMaterialResponseDto
{
    /// <summary>
    /// The unique identifier for the material.
    /// </summary>
    public Guid Id { get; set; }
}