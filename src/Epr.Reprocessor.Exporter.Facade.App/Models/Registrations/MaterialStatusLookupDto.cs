﻿using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

/// <summary>
/// Defines a lookup dto for the details of a materials status.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaterialStatusLookupDto
{
    /// <summary>
    /// The string status for the material.
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// The id of the entry, used to tie entries back together.
    /// </summary>
    public int Id { get; set; }
}