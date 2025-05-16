namespace Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;

using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

public interface IAccreditationPrnIssueAuthService
{
    Task<List<AccreditationPrnIssueAuthDto>> GetByAccreditationId(Guid accreditationId);
    Task ReplaceAllByAccreditationId(Guid accreditationId, List<AccreditationPrnIssueAuthRequestDto> requestDtos);
}
