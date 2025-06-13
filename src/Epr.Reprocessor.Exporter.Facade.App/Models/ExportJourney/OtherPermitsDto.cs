namespace Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney
{
	public class OtherPermitsDto
    {
		public Guid Id { get; set; }

		public int RegistrationId { get; set; }

		public string WasteLicenseOrPermitNumber { get; set; }

		public string PpcNumber { get; set; }

		public List<string> WasteExemptionReference { get; set; } = new List<string>();
	}
}
