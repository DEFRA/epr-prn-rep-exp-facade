

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Addresses;

public class AddressLookupResponse
{
    public AddressLookupResponseHeader Header { get; set; } = default!;
    
    public AddressLookupResponseResult[] Results { get; set; } = default!;
}