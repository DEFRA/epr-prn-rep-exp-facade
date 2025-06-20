using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents the details of a created registration.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreateRegistrationResponseDto
{
    /// <summary>
    /// The ID of the created registration.
    /// </summary>
    public Guid Id { get; set; }
}