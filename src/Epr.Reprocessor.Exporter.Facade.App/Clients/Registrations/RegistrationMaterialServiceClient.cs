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
        var url = string.Format(Endpoints.CreateExemptionReferences, _config.ApiVersion, request.RegistrationMaterialId);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        await PostAsync<CreateExemptionReferencesDto>(url, request);
    }

    public async Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterialAsync(CreateRegistrationMaterialRequestDto request)
    {
        var url = string.Format(Endpoints.CreateRegistrationMaterial, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        return await PostAsync<CreateRegistrationMaterialRequestDto, CreateRegistrationMaterialResponseDto>(url, request);
    }

    public async Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid id, UpdateRegistrationMaterialPermitsDto request)
    {
        logger.LogInformation("Attempting to update an existing registration material with External ID {Id}", id);

        var url = string.Format(Endpoints.UpdateRegistrationMaterialPermits, _config.ApiVersion, id);

        return await PostAsync<UpdateRegistrationMaterialPermitsDto, bool>(url, request);
    }

    public async Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync()
    {
        logger.LogInformation("Attempting to get list of material permit types");

        var url = string.Format(Endpoints.GetMaterialsPermitTypes, _config.ApiVersion);

        return await GetAsync<List<MaterialsPermitTypeDto>>(url);
    }
}