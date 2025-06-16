using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public class RegistrationMaterialServiceClient(
    ILogger<RegistrationMaterialServiceClient> logger,
    HttpClient httpClient,
    IOptions<PrnBackendServiceApiConfig> options) : BaseHttpClient(httpClient), IRegistrationMaterialServiceClient
{
    private readonly ILogger<RegistrationMaterialServiceClient> _logger = logger;
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    public async Task CreateExemptionReferencesAsync(CreateExemptionReferencesDto request)
    {        
        var url = string.Format(Endpoints.RegistrationMaterial.CreateExemptionReferences, _config.ApiVersion, request.RegistrationMaterialId);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        await PostAsync<CreateExemptionReferencesDto>(url, request);
    }

    public async Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterialAsync(CreateRegistrationMaterialRequestDto request)
    {
        var url = string.Format(Endpoints.RegistrationMaterial.CreateRegistrationMaterial, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        return await PostAsync<CreateRegistrationMaterialRequestDto, CreateRegistrationMaterialResponseDto>(url, request);
    }

    public async Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationMaterialsAsync(Guid registrationId)
    {
        var url = string.Format(Endpoints.RegistrationMaterial.GetAllRegistrationMaterials, _config.ApiVersion, registrationId);
        _logger.LogInformation("Calling {Url} to retrieve all registration materials.", url);

        return await GetAsync<List<ApplicationRegistrationMaterialDto>>(url);
    }
}