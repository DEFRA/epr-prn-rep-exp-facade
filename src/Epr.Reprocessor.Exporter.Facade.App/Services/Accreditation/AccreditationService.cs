namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public class AccreditationService(IAccreditationServiceClient serviceClient) : IAccreditationService
{
    public async Task<Guid> GetOrCreateAccreditation(
        Guid organisationId,
        int materialId,
        int applicationTypeId)
    {
        return await serviceClient.GetOrCreateAccreditation(
            organisationId,
            materialId,
            applicationTypeId);
    }

    public async Task<AccreditationDto> GetAccreditationById(Guid accreditationId)
    {
        return await serviceClient.GetAccreditationById(accreditationId);
    }

    public async Task<AccreditationDto> UpsertAccreditation(AccreditationRequestDto requestDto)
    {
        return await serviceClient.UpsertAccreditation(requestDto);
    }
}
