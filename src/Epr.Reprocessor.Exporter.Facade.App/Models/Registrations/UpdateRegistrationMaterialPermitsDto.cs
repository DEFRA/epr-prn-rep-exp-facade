using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Dto to update permit-related information for a registration material permits.
/// </summary>
[ExcludeFromCodeCoverage]
public class UpdateRegistrationMaterialPermitsDto
{
    /// <summary>
    /// Gets or sets the ID of the permit type.
    /// </summary>
    public int? PermitTypeId { get; set; }

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string? PermitNumber { get; set; }
}