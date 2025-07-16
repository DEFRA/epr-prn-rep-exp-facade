using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Dto to update the maximum weight that a site is capable of processing for a material.
/// </summary>
[ExcludeFromCodeCoverage]
public class UpdateMaximumWeightDto
{
    /// <summary>
    /// The maximum weight in tonnes.
    /// </summary>
    public required decimal WeightInTonnes { get; set; }

    /// <summary>
    /// The ID of the period within which the max weight is applicable.
    /// </summary>
    public int PeriodId { get; set; }
}