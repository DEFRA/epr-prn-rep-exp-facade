using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Config;

[ExcludeFromCodeCoverage]
public class AccountsServiceApiConfig
{
    public const string SectionName = "AccountsServiceApiConfig";

    public string BaseUrl { get; set; } = null!;
    public string AccountServiceClientId { get; set; } = null!;
    public string Certificate { get; set; } = null!;
    public int Timeout { get; set; }
    public int ServicePooledConnectionLifetime { get; set; }
    public int ServiceRetryCount { get; set; }
    public AccountsServiceEndpoints Endpoints { get; set; } = null!;
}