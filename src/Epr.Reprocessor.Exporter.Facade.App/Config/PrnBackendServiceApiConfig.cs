﻿using System.Diagnostics.CodeAnalysis;

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

	public PrnServiceApiConfigExportEndpoints ExportEndpoints { get; set; } = null!;
}

[ExcludeFromCodeCoverage]
public class PrnServiceApiConfigEndpoints
{
    public string AccreditationGetOrCreate { get; set; }
    public string AccreditationGet { get; set; }
    public string AccreditationPost { get; set; }
    public string AccreditationPrnIssueAuthGet { get; set; }
    public string AccreditationPrnIssueAuthPost { get; set; }
    public string OverseasAccreditationSiteGet { get; set; }
    public string OverseasAccreditationSitePost { get; set; }
    public string AccreditationFileUploadGet { get; set; }
    public string AccreditationFileUploadsGet { get; set; }
    public string AccreditationFileUploadPost { get; set; }
    public string AccreditationFileUploadDelete { get; set; }
    public string AccreditationOverViewByOrgId { get; set; }
}

[ExcludeFromCodeCoverage]
public class PrnServiceApiConfigExportEndpoints
{
	public string OtherPermitsGet { get; set; }
	public string OtherPermitsPost { get; set; }
    public string OtherPermitsPut { get; set; }

	public string WasteCarrierBrokerDealerRefGet { get; set; }
	public string WasteCarrierBrokerDealerRefPost { get; set; }
	public string WasteCarrierBrokerDealerRefPut { get; set; }
}
