namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public class AccreditationPrnIssueAuthService(IAccreditationPrnIssueAuthServiceClient serviceClient) : IAccreditationPrnIssueAuthService
{
    public async Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId)
    {
        return await serviceClient.GetByAccreditationId(accreditationId);
    }

    public async Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> requestDtos)
    {
        await serviceClient.ReplaceAllByAccreditationId(accreditationId, requestDtos);
    }
}
