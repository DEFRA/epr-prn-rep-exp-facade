using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Represents the details of a registrations tasks and its materials and associated material tasks.
/// </summary>
[ExcludeFromCodeCoverage]
public class ApplicantRegistrationTaskOverviewDto
{
    /// <summary>
    /// The unique identifier foe the registration.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the organisation that this registration is for.
    /// </summary>
    public Guid OrganisationId { get; set; }

    /// <summary>
    /// The name of the regulator that will be processing this registration.
    /// </summary>
    public string Regulator { get; set; }

    /// <summary>
    /// The registration level tasks associated with this registration.
    /// </summary>
    public List<RegistrationTaskDto> Tasks { get; set; } = [];

    /// <summary>
    /// The registration material level tasks associated with this registration, lists on a per-material basis, so each material can have multiple tasks associated with it.
    /// </summary>
    public List<ApplicantRegistrationMaterialTaskOverviewDto> Materials { get; set; } = [];
}