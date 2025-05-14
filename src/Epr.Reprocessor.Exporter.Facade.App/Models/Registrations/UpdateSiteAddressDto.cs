namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class UpdateSiteAddressDto
{
    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    public AddressDto ReprocessingSiteAddress { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the legal document address
    /// </summary>
    public AddressDto LegalDocumentAddress { get; set; }
}
