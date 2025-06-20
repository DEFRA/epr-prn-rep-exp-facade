using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents an individual material using to load all available materials that can be applied for, this is not the same as a <see cref="RegistrationMaterialLookupDto"/> which is used specifically for registered materials associated with a registration.
/// </summary>
[ExcludeFromCodeCoverage]
public record AvailableMaterialDto
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