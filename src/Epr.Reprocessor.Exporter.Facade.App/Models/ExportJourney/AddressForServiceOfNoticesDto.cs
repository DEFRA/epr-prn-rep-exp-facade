using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney
{
    [ExcludeFromCodeCoverage]
    public class AddressForServiceOfNoticesDto
    {
        public AddressDto LegalDocumentAddress { get; set; }

        public Guid RegistrationId { get; set; }
    }
}
