using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

/// <summary>
/// Service interface for managing material exemption references in the reprocessor/exporter journey.
/// </summary>
public interface IMaterialExemptionReferenceService
{
    Task<bool> CreateMaterialExemptionReferencesAsync(CreateMaterialExemptionReferenceDto dto);        
}
