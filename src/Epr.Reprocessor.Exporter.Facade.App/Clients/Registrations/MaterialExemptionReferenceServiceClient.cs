using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

/// <summary>
/// Client for managing material exemption references in the reprocessor/exporter journey.
/// </summary>
public class MaterialExemptionReferenceServiceClient(
    ILogger<MaterialExemptionReferenceServiceClient> logger,
    HttpClient httpClient,
    IOptions<PrnBackendServiceApiConfig> options) : BaseHttpClient(httpClient), IMaterialExemptionReferenceServiceClient
{
    private readonly ILogger<MaterialExemptionReferenceServiceClient> _logger = logger;
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns>bool</returns>    
    public async Task<bool> CreateMaterialExemptionReferencesAsync(CreateMaterialExemptionReferenceDto request)
    {
        var url = string.Format(Endpoints.CreateMaterialExemptionReferences, _config.ApiVersion);
        _logger.LogInformation("Calling {Url} to save materials.", url);        

        return await PostAsync<CreateMaterialExemptionReferenceDto, bool>(url, request);
    }
}
