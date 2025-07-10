namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class UpdateIsMaterialRegisteredDto
{
	public Guid RegistrationMaterialId { get; set; }
	public bool? IsMaterialRegistered { get; set; }
}
