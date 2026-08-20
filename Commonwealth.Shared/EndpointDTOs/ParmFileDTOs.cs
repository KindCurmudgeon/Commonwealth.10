
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EndpointDTOs;

public partial class ParmFileInfo
{
    [JsonInclude] public required string RootName { get; set; }
    [JsonInclude] public required string Version { get; set; }
    [JsonInclude] public required string Type { get; set; }
    [JsonConstructor] public ParmFileInfo() { }
    public static string FileName(string rootName, string type) { return $"{rootName.ToLower()}.{type.ToLower()}.json"; }
    public string FileName() { return FileName(RootName, Type); }

}





