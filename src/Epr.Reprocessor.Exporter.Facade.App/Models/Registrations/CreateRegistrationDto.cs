using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents details for the creation of a registration.
/// </summary>
[ExcludeFromCodeCoverage]
public class CreateRegistrationDto
{
    /// <summary>
    /// The type of the application i.e. Reprocessor, Exporter etc.
    /// </summary>
    [Required]
    public int ApplicationTypeId { get; set; }

    /// <summary>
    /// The unique identifier for the organisation.
    /// </summary>
    [Required]
    public int OrganisationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    [Required]
    public AddressDto ReprocessingSiteAddress { get; set; }
}