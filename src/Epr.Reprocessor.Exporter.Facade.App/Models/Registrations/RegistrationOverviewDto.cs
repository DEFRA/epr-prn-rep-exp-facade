﻿using Epr.Reprocessor.Exporter.Facade.App.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class RegistrationOverviewDto
{
    public int Id { get; set; }

    public required string OrganisationName { get; set; } = string.Empty;

    public string? SiteAddress { get; init; }

    public ApplicationOrganisationType OrganisationType { get; set; }

    public required string Regulator { get; set; }

    public List<RegistrationTaskDto> Tasks { get; set; } = [];

    public List<RegistrationMaterialDto> Materials { get; set; } = [];
}

