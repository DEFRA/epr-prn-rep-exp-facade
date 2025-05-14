namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;

using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public interface IAccreditationPrnIssueAuthServiceClient
{
    Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId);
    Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> requestDtos);
}
