using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

/// <summary>
/// Defines a service to manage materials.
/// </summary>
public interface IMaterialService
{
    /// <summary>
    /// Retrieve all materials that can be applied for.
    /// </summary>
    /// <returns>Collection containing all found materials.</returns>
    Task<IEnumerable<AvailableMaterialDto>> GetAllMaterialsAsync();
}