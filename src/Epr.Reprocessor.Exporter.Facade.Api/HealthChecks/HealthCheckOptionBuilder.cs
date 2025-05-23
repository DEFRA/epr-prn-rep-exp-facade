﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Epr.Reprocessor.Exporter.Facade.Api.HealthChecks;

[ExcludeFromCodeCoverage]
public static class HealthCheckOptionBuilder
{
	public static HealthCheckOptions Build() => new()
	{
		AllowCachingResponses = false,
		ResultStatusCodes =
		{
			[HealthStatus.Healthy] = StatusCodes.Status200OK
		}
	};
}
