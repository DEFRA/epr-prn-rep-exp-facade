namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public interface IAccreditationServiceClient
{
    Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task<AccreditationDto> GetAccreditationById(Guid accreditationId);

    Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto requestDto);

    Task ClearDownDatabase();
}
