using Epr.Reprocessor.Exporter.Facade.App.Enums;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

public class UpdateRegistrationTaskStatusDto
{
    public string TaskName { get; set; } = string.Empty;

    public TaskStatuses Status { get; set; }
}
