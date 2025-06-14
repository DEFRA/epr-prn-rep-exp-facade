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
        var url = string.Format(Endpoints.CreateExemptionReferences, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        await PostAsync<CreateExemptionReferencesDto>(url, request);
    }

    public async Task<Guid> CreateRegistrationMaterialAsync(CreateRegistrationMaterialDto request)
    {
        var url = string.Format(Endpoints.CreateRegistrationMaterial, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to save materials.", url);

        return await PostAsync<CreateRegistrationMaterialDto, Guid>(url, request);
    }
}
