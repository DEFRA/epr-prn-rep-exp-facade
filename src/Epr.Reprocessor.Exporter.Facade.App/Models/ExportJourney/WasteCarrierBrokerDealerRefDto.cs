using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney
{
    [ExcludeFromCodeCoverage]
    public class WasteCarrierBrokerDealerRefDto
    {
        public Guid Id { get; set; }

        public int RegistrationId { get; set; }

        public string WasteCarrierBrokerDealerRef { get; set; }
    }
}
