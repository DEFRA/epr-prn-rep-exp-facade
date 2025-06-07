using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

/// <summary>
/// Service for managing material exemption references in the reprocessor/exporter journey.
/// </summary>
public class MaterialExemptionReferenceService(IMaterialExemptionReferenceServiceClient materialExemptionReferenceServiceClient) : IMaterialExemptionReferenceService
{
    public async Task<bool> CreateMaterialExemptionReferencesAsync(CreateMaterialExemptionReferenceDto dto) 
        => await materialExemptionReferenceServiceClient.CreateMaterialExemptionReferencesAsync(dto);            
    
}
