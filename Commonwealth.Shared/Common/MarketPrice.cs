using System.Text.Json.Serialization;

namespace Commonwealth.Shared.Common;

public partial class MarketPrice
{
    public required string Name { get; set; }
    public required double Price { get; set; }

    [JsonConstructor] public MarketPrice() { }
}