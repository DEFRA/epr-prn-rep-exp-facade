using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

public interface IAccreditationService
{
    Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId);

    Task<AccreditationDto> GetAccreditationById(Guid accreditationId);

    Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto accreditationDto);

    Task ClearDownDatabase();
}
