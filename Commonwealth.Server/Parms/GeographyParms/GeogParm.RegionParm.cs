using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Commonwealth.Server.Parameters;

public class Region
{
    public required string PrimaryName { get; set; }
    public required string PossessiveName { get; set; }
    public string? Color { get; set; }
    [JsonConstructor] public Region() { }
    [SetsRequiredMembers]
    public Region(RegionFile regionFile)
    {
        PrimaryName = regionFile.PrimaryName ?? "Missing";
        PossessiveName = regionFile.PossessiveName ?? PrimaryName;
        Color = regionFile.Color ?? "Black";
    }
}
public class RegionFile
{
    public string? PrimaryName { get; set; }
    public string? PossessiveName { get; set; }
    public string? Color { get; set; }
    [JsonConstructor] public RegionFile() { }
}