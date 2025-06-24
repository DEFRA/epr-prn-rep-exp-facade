using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney
{
    [ExcludeFromCodeCoverage]
    public class CarrierBrokerDealerPermitsDto
    {
		public Guid CarrierBrokerDealerPermitId { get; set; }

		public Guid RegistrationId { get; set; }

		public string WasteLicenseOrPermitNumber { get; set; }

		public string PpcNumber { get; set; }

		public List<string> WasteExemptionReference { get; set; } = new List<string>();
	}
}
