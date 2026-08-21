using System.Text.Json.Serialization;

namespace Commonwealth.Server.Parameters;

public partial class DistrictParm
{
    public required string Name { get; set; }

  // public required string Possessive {get;set;}

    public string? Region { get; set; }
    public List<string>? LandConnections { get; set; }
    public List<string>? SeaConnections { get; set; }
    public List<string>? Features { get; set; }

    [JsonConstructor] public DistrictParm() { }
}
