using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

/// <summary>
/// Client interface for managing material exemption references in the reprocessor/exporter journey.
/// </summary>
public interface IMaterialExemptionReferenceServiceClient
{
    /// <summary>
    /// Creates a material exemption reference in the reprocessor/exporter journey.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> CreateMaterialExemptionReferencesAsync(CreateMaterialExemptionReferenceDto request);    
}
