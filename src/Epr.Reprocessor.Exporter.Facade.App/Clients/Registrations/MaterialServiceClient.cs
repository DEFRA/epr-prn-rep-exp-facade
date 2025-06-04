namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

/// <summary>
/// Implementation of the material service client that communicates with the backend API to retrieve materials.
/// </summary>
/// <param name="logger">The logger to log it.</param>
/// <param name="client">The wrapped http client instance that communicates with the backend API.</param>
/// <param name="options">Strongly typed configuration object.</param>
public class MaterialServiceClient(
    ILogger<MaterialServiceClient> logger,
    HttpClient client,
    IOptions<PrnBackendServiceApiConfig> options)
    : BaseHttpClient(client), IMaterialServiceClient
{
    private readonly ILogger<MaterialServiceClient> _logger = logger;
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    /// <inheritdoc />>.
    public async Task<IEnumerable<MaterialDto>> GetAllMaterialsAsync()
    {
        var url = string.Format(Endpoints.Material.GetAllMaterials, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to retrieve materials.", url);

        return await GetAsync<IEnumerable<MaterialDto>>(url);
    }
}