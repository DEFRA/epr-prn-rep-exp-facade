using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney
{
    [ExcludeFromCodeCoverage]
    public class WasteCarrierBrokerDealerRefDto
    {
        public Guid CarrierBrokerDealerPermitId { get; set; }

        public Guid RegistrationId { get; set; }

        public string WasteCarrierBrokerDealerRegistration { get; set; }
    }
}
