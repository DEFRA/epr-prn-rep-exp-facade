using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;

[ExcludeFromCodeCoverage]
public class OverseasAddressContact
{
    [MaxLength(100)]
    public required string FullName { get; set; }
    [MaxLength(100)]
    public required string Email { get; set; }
    [MaxLength(25)]
    public required string PhoneNumber { get; set; }
}
