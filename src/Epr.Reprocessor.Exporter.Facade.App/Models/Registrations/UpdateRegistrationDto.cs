using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class UpdateRegistrationDto
{
    /// <summary>
    /// Gets or sets the business address
    /// </summary>
    public AddressDto BusinessAddress { get; set; } = new();

    /// <summary>
    /// Gets or sets the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; } = new();

    /// <summary>
    /// Gets or sets the legal address
    /// </summary>
    public AddressDto LegalAddress { get; set; } = new();
}