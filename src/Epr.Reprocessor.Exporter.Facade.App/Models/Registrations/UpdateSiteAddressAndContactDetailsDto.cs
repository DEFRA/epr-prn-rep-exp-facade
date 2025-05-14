namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class UpdateSiteAddressAndContactDetailsDto
{
    /// <summary>
    /// Gets or sets registration id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the reprocessing site address
    /// </summary>
    public int? ReprocessingSiteAddressId { get; set; }

    public AddressDto? ReprocessingSiteAddress { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the legal document address
    /// </summary>
    public int? LegalDocumentAddressId { get; set; }

    public AddressDto? LegalDocumentAddress { get; set; }
}
