using System.Text.Json.Serialization;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EndpointDTOs;


namespace Commonwealth.Server.Parameters;

public partial class GeographyFile : IBlobObject
{
    public required ParmFileInfo ParmFileInfo { get; set; }
    //   public required string WorldName { get; set; }
    //   public string? Version { get; set; }
    public required List<RegionFile> Regions { get; set; }
    // public List<string>? LeaderTitles { get; set; }
    public required List<DistrictParm> Districts { get; set; }
    public required List<DistrictParm>? Seas { get; set; }
    [JsonConstructor] public GeographyFile() { }

    public static ParmFileInfo DefaultFileInfo = new()
    {
        RootName = "test",
        Version = "0.0",
        Type = ParmFileType.Geog
    };

    // public List<ConnectionError> ValidateWorldFileContent()
    // {
    //     List<ConnectionError> errors = [];
    //     foreach (DistrictParm districtParm in Districts ?? [])
    //     {
    //         foreach (string connection in districtParm.LandConnections ?? [])
    //         {
    //             DistrictParmFile? other = Districts?.Find(d => d.Name == connection);
    //             if (other is null)
    //             {
    //                 errors.Add(new ConnectionError(districtParm.Name, connection));
    //             }
    //             if (other is not null)
    //             {
    //                 string? confirmed = other.LandConnections?.Find(c => c == districtParm.Name);
    //                 if (confirmed is null)
    //                 {
    //                     errors.Add(new ConnectionError(other.Name, districtParm.Name));
    //                 }
    //             }
    //         }
    //         foreach (string connection in districtParm.SeaConnections ?? [])
    //         {
    //             DistrictParmFile? other = Seas?.Find(d => d.Name == connection);
    //             if (other is null)
    //             {
    //                 errors.Add(new ConnectionError(districtParm.Name, connection));
    //             }
    //             if (other is not null)
    //             {
    //                 string? confirmed = other.LandConnections?.Find(c => c == districtParm.Name);
    //                 if (confirmed is null)
    //                 {
    //                     errors.Add(new ConnectionError(other.Name, districtParm.Name));
    //                 }
    //             }
    //         }
    //     }
    //     foreach (DistrictParmFile seaParm in Seas ?? [])
    //     {
    //         foreach (string connection in seaParm.SeaConnections ?? [])
    //         {
    //             DistrictParmFile? other = Seas?.Find(d => d.Name == connection);
    //             if (other is null)
    //             {
    //                 errors.Add(new ConnectionError(seaParm.Name, connection));
    //             }
    //             if (other is not null)
    //             {
    //                 string? confirmed = other.SeaConnections?.Find(c => c == seaParm.Name);
    //                 if (confirmed is null)
    //                 {
    //                     errors.Add(new ConnectionError(other.Name, seaParm.Name));
    //                 }
    //             }
    //         }
    //     }
    //     return errors;
    // }

    // public DistrictParmFile? FindDistrict(string name)
    // {
    //     return Districts?.Find(d => d.Name == name);
    // }
    // public DistrictParmFile? FindSea(string name)
    // {
    //     return Seas?.Find(s => s.Name == name);
    // }

    // public List<DistrictParmFile> GetElgibleDistricts(List<string>? constraints)
    // {
    //     List<DistrictParmFile> elgible = [.. Districts ?? []];
    //     if (constraints is not null)
    //     {
    //         foreach (string constraint in constraints)
    //         {
    //             elgible.RemoveAll(dd => dd.HasFeature(constraint) is false);
    //         }
    //     }
    //     return elgible;
    // }
    // public List<string> GetRegionNames()
    // {
    //     List<string> Names = [];
    //     foreach (Region region in Regions??[]) {
    //         string? name = region.PrimaryName;
    //         if (name != null) Names.Add(name);
    //     }
    //     return Names;
    // }

    // public List<DistrictParm> FindAllWithFeature(string feature)
    // {
    //     List<DistrictParm> result = [.. Districts ?? []];
    //     result.RemoveAll(dd => dd.HasFeature(feature) is false);
    //     return result;
    // }

    // public string? GetRandomLeaderTitle()
    // {
    //     return Util.PickRandomFromList<string>(LeaderTitles);
    // }
}

public class ConnectionError(string? district, string? other)
{
    public string? District { get; set; } = district;
    public string? Other { get; set; } = other;
}


