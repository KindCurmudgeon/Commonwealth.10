using System.Text.Json.Serialization;
using Commonwealth.Server.Data;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Parameters;

public partial class NamingFile : IBlobObject
{
    public required ParmFileInfo ParmFileInfo { get; set; }
    public required List<string> LeaderTitles { get; set; }
    public required List<NationNameOption> NationNames { get; set; }
    public List<string>? Governments { get; set; }
    // public const string FileRootName = "Naming";



}
public record NationNameOption(string Name, string Possessive);


