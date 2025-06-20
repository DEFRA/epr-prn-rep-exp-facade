using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Defines a lookup dto for the details of a singular registration material including its name and ID.
/// </summary>
[ExcludeFromCodeCoverage]
public record RegistrationMaterialLookupDto
{
    /// <summary>
    /// The name of the material.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The id of the entry, used to tie entries back together.
    /// </summary>
    public int Id { get; set; }
}