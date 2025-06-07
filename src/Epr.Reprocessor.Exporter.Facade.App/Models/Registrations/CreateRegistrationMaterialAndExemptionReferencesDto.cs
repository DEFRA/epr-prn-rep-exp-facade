namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class CreateRegistrationMaterialAndExemptionReferencesDto
{
    /// <summary>
    /// Gets or sets the list of material exemption references.
    /// </summary>
    public List<MaterialExemptionReferenceDto> MaterialExemptionReferences { get; set; }
    /// <summary>
    /// Gets or sets the registration material.
    /// </summary>
    public RegistrationMaterialDto RegistrationMaterial { get; set; }
}
