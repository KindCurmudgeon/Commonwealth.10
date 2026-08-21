
using System.Text.Json.Serialization;

namespace Commonwealth.Shared.EndpointDTOs;

public class ParmFileInfo
{
    public required string RootName { get; set; }
    public required string Version { get; set; }
    public required string Type { get; set; }
    public static string FileName(string rootName, string type) { return $"{rootName.ToLower()}.{type.ToLower()}.json"; }
    public string FileName() { return FileName(RootName, Type); }
}





