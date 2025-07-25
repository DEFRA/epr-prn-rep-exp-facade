﻿using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;

[ExcludeFromCodeCoverage]
public class OverseasAddressWasteCodes
{
    public Guid ExternalId { get; set; }
    public required string CodeName { get; set; }
}
