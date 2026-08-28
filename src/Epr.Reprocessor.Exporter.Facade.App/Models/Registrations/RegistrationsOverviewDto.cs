#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationsOverviewDto

    {
        public Guid Id { get; set; }

        public int RegistrationMaterialId { get; set; }

        public int MaterialId { get; set; }

        public string? Material { get; set; }

        public string? MaterialCode { get; set; }

        public int ApplicationTypeId { get; set; }

        public string? ApplicationType { get; set; }

        public int RegistrationStatus { get; set; }

        public int AccreditationStatus { get; set; }

        public int? ReprocessingSiteId { get; set; }

        public AddressDto? ReprocessingSiteAddress { get; set; }

        public int? RegistrationYear { get; set; }

        public int? AccreditationYear { get; set; }
    }
}
