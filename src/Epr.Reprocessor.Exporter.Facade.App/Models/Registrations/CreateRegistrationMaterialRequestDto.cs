namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class CreateRegistrationMaterialRequestDto
{
    public Guid RegistrationId { get; set; }

    public string Material { get; set; }
}
