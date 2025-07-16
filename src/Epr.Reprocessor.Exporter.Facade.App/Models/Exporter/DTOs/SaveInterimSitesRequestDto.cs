namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;
public class SaveInterimSitesRequestDto
{
    public Guid RegistrationMaterialId { get; set; }
    public required List<OverseasMaterialReprocessingSiteDto> OverseasMaterialReprocessingSites { get; set; }
    public Guid? UserId { get; set; }
}
