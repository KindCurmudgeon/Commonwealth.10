using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;

public partial class DistrictGeography
{
    public required string Name { get; set; }
   // public required string Possessive { get; set; }
    public required string Region { get; set; }
    public required List<string> Connections { get; set; }
    public List<Feature>? Features { get; set; }
    [JsonConstructor] public DistrictGeography() { }

}