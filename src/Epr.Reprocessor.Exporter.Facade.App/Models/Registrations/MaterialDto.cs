using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents an individual material.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaterialDto
{
    /// <summary>
    /// The name of the material e.g. "Plastic", "Metal", etc.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The shorthand code for the material.
    /// </summary>
    public string Code { get; set; } = null!;
}