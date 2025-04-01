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
    }
}
