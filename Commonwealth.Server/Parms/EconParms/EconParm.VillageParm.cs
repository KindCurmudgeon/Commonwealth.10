using System.Text.Json.Serialization;
using Commonwealth.Shared.Common;


namespace Commonwealth.Server.Parameters;


// public partial class VillageParm
// {
//     public VillageParm(VillageParmFile? villageParmFile)
//     {
//         Name = villageParmFile?.Type ?? throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, "VillageParm: Type");
//     //    Abbr = villageParmFile?.Abbr ?? Name;
//         Class = villageParmFile?.Class ?? throw new AppException(ExceptionType.Blob, BlobFailType.FileContent, "VillageParm: Class");
//         Construction = villageParmFile?.Construction;
//         Operation = villageParmFile?.Operation ?? Defaults.Operation;
//         Production = villageParmFile?.Production;
//         ProductionType = Util.StringToEnum<ProductionType>(villageParmFile?.ProductionType);
//         Resource = villageParmFile?.Resource;
//         Constraint = Util.StringToEnum<Feature>(villageParmFile?.Constraint);
//         MaxWorkers = villageParmFile?.MaxWorkers ?? Defaults.MaxWorkers;
//     }
//     private static class Defaults
//     {
//         public static List<Asset> Operation = new List<Asset> { new Asset("Wood", 5), new Asset("Gems", 300) };
//         public static int MaxWorkers = 300;
//     }
// }
public class VillageParmFile
{
    public string? Type { get; set; }
    public string? Abbr { get; set; }
    public string? Class { get; set; }  // Production, Trading, Storage
                                        //   public int? Size { get; set; 
    public Asset? Production { get; set; }
    public string? ProductionType { get; set; }
    public string? Resource { get; set; }
    public string? Constraint { get; set; }
    public List<Asset>? Construction { get; set; }
    public List<Asset>? Operation { get; set; }
    public int? MaxWorkers { get; set; }
    [JsonConstructor] public VillageParmFile() { }
}

