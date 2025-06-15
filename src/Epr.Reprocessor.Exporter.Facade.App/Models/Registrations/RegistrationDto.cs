using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class RegistrationDto
{
    public Guid Id { get; set; }

    public int ApplicationTypeId { get; set; }

    public Guid OrganisationId { get; set; }

    public int RegistrationStatusId { get; set; }

    public AddressDto? BusinessAddress { get; set; }

    public AddressDto? ReprocessingSiteAddress { get; set; }

    public AddressDto? LegalDocumentAddress { get; set; }

    public IList<RegistrationTaskDto> Tasks { get; set; }
}