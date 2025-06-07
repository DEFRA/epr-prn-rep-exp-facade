namespace Epr.Reprocessor.Exporter.Facade.App.Models;

public class MaterialExemptionReferenceDto
{
    public Guid ExternalId { get; set; }

    public int RegistrationMaterialId { get; set; }

    public string ReferenceNumber { get; set; }
}
