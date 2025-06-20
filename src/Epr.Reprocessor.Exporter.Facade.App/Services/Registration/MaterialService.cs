using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

/// <summary>
/// Implementation of the material service that interacts with the backend API to retrieve materials.
/// </summary>
/// <param name="materialServiceClient">Wrapper client to communicate with the backend materials API.</param>
public class MaterialService(IMaterialServiceClient materialServiceClient) : IMaterialService
{
    /// <inheritdoc />.
    public async Task<IEnumerable<AvailableMaterialDto>> GetAllMaterialsAsync()
        => await materialServiceClient.GetAllMaterialsAsync();
}