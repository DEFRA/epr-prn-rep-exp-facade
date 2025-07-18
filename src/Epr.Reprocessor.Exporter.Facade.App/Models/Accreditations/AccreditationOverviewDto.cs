
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations
{
    [ExcludeFromCodeCoverage]
    public class AccreditationOverviewDto
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public Guid OrganisationId { get; set; }
        public int RegistrationMaterialId { get; set; }
        public int ApplicationTypeId { get; set; }
        public int AccreditationStatusId { get; set; }
        public string? DecFullName { get; set; }
        public string? DecJobTitle { get; set; }
        public string? AccreferenceNumber { get; set; }

        public int? AccreditationYear { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public RegistrationMaterialDto? RegistrationMaterial { get; set; }
    }
}
