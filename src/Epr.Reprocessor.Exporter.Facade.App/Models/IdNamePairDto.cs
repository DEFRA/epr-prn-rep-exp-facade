using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models;

[ExcludeFromCodeCoverage]
public class IdNamePairDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
