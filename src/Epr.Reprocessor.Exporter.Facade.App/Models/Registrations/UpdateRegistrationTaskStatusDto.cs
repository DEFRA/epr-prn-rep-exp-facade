using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Enums;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

[ExcludeFromCodeCoverage]
public class UpdateRegistrationTaskStatusDto
{
    public string TaskName { get; set; } = string.Empty;

    public TaskStatuses Status { get; set; }
}