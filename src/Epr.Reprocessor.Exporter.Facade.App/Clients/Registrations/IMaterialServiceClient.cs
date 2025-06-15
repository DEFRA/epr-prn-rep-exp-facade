namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

/// <summary>
/// Defines a named HTTP client interface for interacting with the material service API.
/// </summary>
public interface IMaterialServiceClient
{
    /// <summary>
    /// Retrieves all materials that can be applied for from the backend API.
    /// </summary>
    /// <returns>Collection containing all found materials.</returns>
    Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync();
}