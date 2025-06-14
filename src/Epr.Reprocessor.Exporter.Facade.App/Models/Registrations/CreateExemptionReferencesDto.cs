namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class CreateExemptionReferencesDto
{
    /// <summary>
    /// Gets or sets the registration identifier.
    /// </summary>
    public Guid RegistrationMaterialId { get; set; }

    /// <summary>
    /// Gets or sets the list of material exemption references.
    /// </summary>
    public List<MaterialExemptionReferenceDto> MaterialExemptionReferences { get; set; }     
}
