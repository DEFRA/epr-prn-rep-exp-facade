using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Config;

[ExcludeFromCodeCoverage]
public class PrnBackendServiceApiConfig
{
    public const string SectionName = "PrnBackendServiceApiConfig";

    public string BaseUrl { get; set; } = null!;

    public int Timeout { get; set; }

    public string ClientId { get; set; } = string.Empty;

    public int ApiVersion { get; set; }

    public int ServiceRetryCount { get; set; }

    public PrnServiceApiConfigEndpoints Endpoints { get; set; } = null!;
}

public class PrnServiceApiConfigEndpoints
{
    public string UpdateSiteAddressAndContactDetails { get; set; }
}
