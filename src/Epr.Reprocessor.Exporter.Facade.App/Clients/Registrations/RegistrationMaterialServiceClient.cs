﻿using Epr.Reprocessor.Exporter.Facade.App.Config;
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

    public async Task<bool> UpdateRegistrationMaterialPermitsAsync(Guid id, UpdateRegistrationMaterialPermitsDto request)
    {
        _logger.LogInformation("Attempting to update an existing registration material permits with External ID {Id}", id);

        var url = string.Format(Endpoints.RegistrationMaterial.UpdateRegistrationMaterialPermits, _config.ApiVersion, id);

        await PostAsync<UpdateRegistrationMaterialPermitsDto>(url, request);

        return true;
    }

    public async Task<bool> UpdateRegistrationMaterialPermitCapacityAsync(Guid id, UpdateRegistrationMaterialPermitCapacityDto request)
    {
        _logger.LogInformation("Attempting to update an existing registration material permit capacity with External ID {Id}", id);

        var url = string.Format(Endpoints.RegistrationMaterial.UpdateRegistrationMaterialPermitCapacity, _config.ApiVersion, id);

        await PostAsync<UpdateRegistrationMaterialPermitCapacityDto>(url, request);

        return true;
    }

    public async Task<List<MaterialsPermitTypeDto>> GetMaterialsPermitTypesAsync()
    {
        _logger.LogInformation("Attempting to get list of material permit types");

        var url = string.Format(Endpoints.RegistrationMaterial.GetMaterialsPermitTypes, _config.ApiVersion);

        return await GetAsync<List<MaterialsPermitTypeDto>>(url);
    }

    public async Task<List<ApplicationRegistrationMaterialDto>> GetAllRegistrationMaterialsAsync(Guid registrationId)
    {
        var url = string.Format(Endpoints.RegistrationMaterial.GetAllRegistrationMaterials, _config.ApiVersion, registrationId);
        _logger.LogInformation("Calling {Url} to retrieve all registration materials.", url);

        return await GetAsync<List<ApplicationRegistrationMaterialDto>>(url);
    }

    public async Task<bool> DeleteAsync(Guid registrationMaterialId)
    {
        var url = string.Format(Endpoints.RegistrationMaterial.Delete, _config.ApiVersion, registrationMaterialId);

        _logger.LogInformation("Calling {Url} to delete registration material.", url);

        return await DeleteAsync(url);
    }
}