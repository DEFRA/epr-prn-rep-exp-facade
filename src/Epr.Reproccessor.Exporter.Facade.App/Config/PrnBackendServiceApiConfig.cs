using System.Diagnostics.CodeAnalysis;

namespace Epr.Reproccessor.Exporter.Facade.App.Config
{
    [ExcludeFromCodeCoverage]
    public class PrnBackendServiceApiConfig
    {
        public const string SectionName = "PrnBackendServiceApiConfig";

        public string BaseUrl { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string Certificate { get; set; } = null!;
        public int Timeout { get; set; }

        public PrnBackendServiceEndpoint Endpoints { get; set; } = null!;
    }

    public class PrnBackendServiceEndpoint
    {
        public string SaveAndContinueSaveUri { get; set; } = "api/v1.0/saveandcontinue";
        public string SaveAndContinueGetLatestUri { get; set; } = "api/v1.0/saveandcontinue/getLatest/";
        public string SaveAndContinueGetAllUri { get; set; } = "api/v1.0/saveandcontinue/getAll/";
    }
}
